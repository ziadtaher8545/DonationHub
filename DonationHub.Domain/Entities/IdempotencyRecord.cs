using DonationHub.Domain.Common.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class IdempotencyRecord : BaseEntity
    {


        public Guid UserId { get; private set; }

        public string RequestHash { get; private set; } = default!;

        public int? ResponseStatusCode { get; private set; }

        public string? ResponseBody { get; private set; }

        public DateTimeOffset EndDate { get; private set; }

        public static IdempotencyRecord Create(Guid userId, string requestHash, int? responseStatusCode, string? responseBody, DateTimeOffset endDate)
        {
            //if (endDate <= DateTimeOffset.UtcNow)
            //    return;

            return new IdempotencyRecord
            {
                UserId = userId,
                RequestHash = requestHash,
                ResponseStatusCode = responseStatusCode,
                ResponseBody = responseBody
            };
        }
    }
}
