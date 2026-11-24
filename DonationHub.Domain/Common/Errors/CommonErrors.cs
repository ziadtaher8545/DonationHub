using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class CommonErrors
    {
        public static readonly Error RateLimited = new("Common.RateLimited", "Too many requests. Please try again later.", ErrorType.TooManyRequests);
    }
}
