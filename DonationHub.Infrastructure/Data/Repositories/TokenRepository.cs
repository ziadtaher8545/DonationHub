using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Data.Repositories;

internal class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
{
    private readonly TimeProvider _timeProvider;

    public TokenRepository(DonationDBContext context, TimeProvider timeProvider) : base(context)
    {
        _timeProvider = timeProvider;
    }

    public async Task<Result<RefreshToken>> GetByTokenHashAsync(string tokenHash, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            return RefreshTokenErrors.TokenHashRequired;

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                ct);

        if (refreshToken is null)
            return RefreshTokenErrors.Invalid;

        return refreshToken;
    }

    public async Task<Result> RevokeFamilyAsync(Guid familyId, CancellationToken ct)
    {
        if (familyId == Guid.Empty)
            return RefreshTokenErrors.FamilyIdRequired;

        var tokens = await _context.RefreshTokens
            .Where(x => x.FamilyId == familyId &&
                        x.RevokedAt == null)
            .ToListAsync(ct);

        var now = _timeProvider.GetUtcNow();

        foreach (var token in tokens)
        {
            var result = token.Revoke(now);

            if (!result.IsSuccess)
                return result.Error;
        }

        return Result.Ok();
    }
}
