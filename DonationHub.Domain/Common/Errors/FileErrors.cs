using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class FileErrors
    {
        public static readonly Error InvalidType = new("File.InvalidType", "The uploaded file type is not allowed.", ErrorType.Validation);

        public static readonly Error TooLarge = new("File.TooLarge", "The uploaded file exceeds the maximum allowed size.", ErrorType.PayloadTooLarge);
    }
}
