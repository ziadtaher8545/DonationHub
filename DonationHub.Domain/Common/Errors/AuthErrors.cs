using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Invalid email or password.", ErrorType.Unauthorized);

        public static readonly Error EmailAlreadyExists = new("Auth.EmailAlreadyExists", "An account with this email already exists.", ErrorType.Conflict);

        public static readonly Error RefreshTokenInvalid = new("Auth.RefreshTokenInvalid", "The refresh token is invalid, expired, or revoked.", ErrorType.Unauthorized);

        public static readonly Error AccountDeactivated = new("Auth.AccountDeactivated", "This account has been deactivated.", ErrorType.Forbidden);
        public static readonly Error Unauthorized = new("Campaign.Unauthorized", "You are not authorized to cancel this campaign.", ErrorType.Unauthorized);

    }
}
