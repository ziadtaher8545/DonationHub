using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class IdempotencyErrors
    {
        public static readonly Error KeyReusedWithDifferentRequest = new("Idempotency.KeyReusedWithDifferentRequest", "The same idempotency key cannot be reused with a different request.", ErrorType.UnprocessableEntity);
    }
}
