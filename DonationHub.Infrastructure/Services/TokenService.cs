using DonationHub.Application.Common.Options;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DonationHub.Infrastructure.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtoptions;
        public TokenService(IOptions<JwtOptions> jwtoptions)
        {
            _jwtoptions = jwtoptions.Value;

        }


        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userId),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Name,userName)
            };


            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            if (string.IsNullOrWhiteSpace(_jwtoptions.SecretKey))
                throw new InvalidOperationException("JWT SecretKey is missing");

            if (_jwtoptions.SecretKey.Length < 32)
                throw new InvalidOperationException("JWT SecretKey is too short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtoptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtoptions.Issuer, audience: _jwtoptions.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_jwtoptions.ExpiryMinutes), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public (string, RefreshToken) CreateRefreshToken(Guid userId)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            var plainText = Convert.ToBase64String(randomBytes);

            // Hashing

            var hashText = HashToken(plainText);


            var refreshToken = RefreshToken.Create(userId, hashText, DateTimeOffset.UtcNow.AddDays(_jwtoptions.RefreshTokenExpiryDays));


            return (plainText, refreshToken);

        }

        public string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);

            var hashBytes = SHA256.HashData(bytes);

            var hashText = Convert.ToHexString(hashBytes);
            return hashText;
        }
    }
}
