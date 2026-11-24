
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;

namespace DonationHub.Domain.Entities;


public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = default!;

    public DateTimeOffset? RevokedAt { get; private set; }

    public DateTimeOffset ExpiredAt { get; private set; }

    public Guid FamilyId { get; private set; }

    public Guid? ReplaceByTokenId { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    private RefreshToken()
    {
    }

    private RefreshToken(Guid userId, string tokenHash, DateTimeOffset expiredAt, Guid familyId)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiredAt = expiredAt;
        FamilyId = familyId;
    }

    public bool IsExpired(DateTimeOffset now)
        => ExpiredAt <= now;

    public bool IsActive(DateTimeOffset now)
        => !IsRevoked && !IsExpired(now);

    public Result Revoke(DateTimeOffset now, Guid? replacedByTokenId = null)
    {
        if (IsRevoked)
            return RefreshTokenErrors.AlreadyRevoked;

        if (replacedByTokenId == Id)
            return RefreshTokenErrors.Invalid;

        RevokedAt = now;
        ReplaceByTokenId = replacedByTokenId;

        return Result.Ok();
    }

    public Result UpdateExpiresAt(DateTimeOffset newExpiration)
    {
        if (newExpiration <= DateTimeOffset.UtcNow)
            return RefreshTokenErrors.ExpirationMustBeInFuture;

        ExpiredAt = newExpiration;

        return Result.Ok();
    }

    public static Result<RefreshToken> Create(Guid userId, string tokenHash, DateTimeOffset expiredAt, Guid familyId, DateTimeOffset now)
    {
        if (userId == Guid.Empty)
            return RefreshTokenErrors.UserIdRequired;

        if (string.IsNullOrWhiteSpace(tokenHash))
            return RefreshTokenErrors.TokenHashRequired;

        if (familyId == Guid.Empty)
            return RefreshTokenErrors.FamilyIdRequired;

        if (expiredAt <= now)
            return RefreshTokenErrors.ExpirationMustBeInFuture;

        var refreshToken = new RefreshToken(userId, tokenHash, expiredAt, familyId);

        return refreshToken;
    }
}
