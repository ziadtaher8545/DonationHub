namespace DonationHub.Domain.Common.Errors;

public static class RefreshTokenErrors
{
    public static readonly Error UserIdRequired =
        new(
            "RefreshToken.UserIdRequired",
            "User ID is required.",
            ErrorType.Validation
        );
    public static readonly Error Invalid = new(
            "RefreshToken.UserIdRequired",
           "Invalid Operation.",
            ErrorType.Validation
        );

    public static readonly Error TokenHashRequired =
        new(
            "RefreshToken.TokenHashRequired",
            "Token hash is required.",
            ErrorType.Validation
        );

    public static readonly Error ExpirationMustBeInFuture =
        new(
            "RefreshToken.ExpirationMustBeInFuture",
            "Expiration date must be in the future.",
            ErrorType.Validation
        );

    public static readonly Error FamilyIdRequired =
        new(
            "RefreshToken.FamilyIdRequired",
            "Token family ID is required.",
            ErrorType.Validation
        );

    public static readonly Error AlreadyRevoked =
        new(
            "RefreshToken.AlreadyRevoked",
            "Refresh token has already been revoked.",
            ErrorType.Conflict
        );

    public static readonly Error TokenExpired =
        new(
            "RefreshToken.Expired",
            "Refresh token has expired.",
            ErrorType.Unauthorized
        );
}