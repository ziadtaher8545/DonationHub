
using Azure.Core;
using DonationHub.Application.Common.Options;
using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.Entities;
using DonationHub.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DonationHub.Infrastructure.Services;

internal class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly TimeProvider _timeProvider;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider, ITokenRepository tokenRepository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _jwtOptions = jwtOptions.Value;
        _timeProvider = timeProvider;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public Result<string> CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
    {
        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(userName) ||
            roles is null)
        {
            return TokenErrors.InvalidUserData;
        }

        if (string.IsNullOrWhiteSpace(_jwtOptions.SecretKey) ||
            Encoding.UTF8.GetByteCount(_jwtOptions.SecretKey) < 32 ||
            string.IsNullOrWhiteSpace(_jwtOptions.Issuer) ||
            string.IsNullOrWhiteSpace(_jwtOptions.Audience) ||
            _jwtOptions.ExpiryMinutes <= 0)
        {
            return TokenErrors.InvalidConfiguration;
        }

        var now = _timeProvider.GetUtcNow();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, userName)
        };

        claims.AddRange(
            roles
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: now.AddMinutes(_jwtOptions.ExpiryMinutes).UtcDateTime,
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }

    public Result<(string, RefreshToken)> CreateRefreshToken(Guid userId, Guid familyId)
    {
        if (userId == Guid.Empty)
            return RefreshTokenErrors.UserIdRequired;

        if (familyId == Guid.Empty)
            return RefreshTokenErrors.FamilyIdRequired;

        if (_jwtOptions.RefreshTokenExpiryDays <= 0)
            return TokenErrors.InvalidConfiguration;

        var now = _timeProvider.GetUtcNow();

        var randomBytes = RandomNumberGenerator.GetBytes(64);

        var plainText = Convert.ToBase64String(randomBytes);

        var hashResult = HashToken(plainText);

        if (!hashResult.IsSuccess)
            return hashResult.Error;

        var expiresAt = now.AddDays(
            _jwtOptions.RefreshTokenExpiryDays);

        var refreshTokenResult = RefreshToken.Create(
            userId,
            hashResult.Data,
            expiresAt,
            familyId, now);

        if (!refreshTokenResult.IsSuccess)
            return refreshTokenResult.Error!;

        return (plainText, refreshTokenResult.Data)!;
    }

    public Result<string> HashToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return TokenErrors.InvalidToken;

        var bytes = Encoding.UTF8.GetBytes(token);

        var hashBytes = SHA256.HashData(bytes);

        var hashText = Convert.ToHexString(hashBytes);

        return hashText;
    }

    public async Task<Result<string>> ActivateRefreshToken(string oldRefreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(oldRefreshToken))
            return AuthErrors.RefreshTokenInvalid;

        var tokenHash = HashToken(oldRefreshToken);

        var oldTokenResult = await _tokenRepository.GetByTokenHashAsync(tokenHash.Data!, ct);

        if (!oldTokenResult.IsSuccess)
            return oldTokenResult.Error!;
        var oldToken = oldTokenResult.Data;

        var now = _timeProvider.GetUtcNow();

        if (oldToken.RevokedAt.HasValue)
        {
            await _tokenRepository.RevokeFamilyAsync(oldToken.FamilyId, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return AuthErrors.RefreshTokenInvalid;

        }

        if (oldToken.ExpiredAt <= now)
            return AuthErrors.RefreshTokenInvalid;


        var user = await _userManager.FindByIdAsync(oldToken.UserId.ToString());

        if (user == null)
            return AuthErrors.InvalidCredentials;

        var refreshTokenResult = CreateRefreshToken(oldToken.UserId, oldToken.FamilyId);

        if (!refreshTokenResult.IsSuccess)
            return refreshTokenResult.Error!;

        var (plaintext, newRefreshToken) = refreshTokenResult.Data;

        var revokeResult = oldToken.Revoke(now, newRefreshToken.Id);
        if (!revokeResult.IsSuccess) return revokeResult.Error!;

        await _tokenRepository.CreateAsync(newRefreshToken, ct);


        await _unitOfWork.SaveChangesAsync(ct);
        return plaintext;

    }
}
