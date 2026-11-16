using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {


        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTimeOffset? RevokedAt { get; private set; }
        public DateTimeOffset ExpiredAt { get; private set; }


        public Guid? ReplaceByTokenId { get; private set; }


        public void Revoke(Guid? replacedByTokenId = null)
        {
            RevokedAt = DateTimeOffset.UtcNow;
            if (replacedByTokenId.HasValue)
                ReplaceByTokenId = replacedByTokenId;
        }
        public void UpdateExpiresIt(DateTimeOffset date)
        {
            ExpiredAt = date;
        }


        public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiredAt)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User Id Can't be null", nameof(userId));

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Token Hash required", nameof(tokenHash));

            if (expiredAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Expire date must be in the future", nameof(expiredAt));


            return new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiredAt = expiredAt
            };
        }
    }
}
