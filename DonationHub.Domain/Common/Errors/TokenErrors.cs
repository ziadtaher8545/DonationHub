namespace DonationHub.Domain.Common.Errors;

public static class TokenErrors
{
    public static readonly Error InvalidUserData =
        new(
            "Token.InvalidUserData",
            "Required user information is missing.",
            ErrorType.Validation);

    public static readonly Error InvalidConfiguration =
        new(
            "Token.InvalidConfiguration",
            "JWT configuration is invalid.",
            ErrorType.Failure);

    public static readonly Error InvalidToken =
        new(
            "Token.Invalid",
            "Token value is invalid.",
            ErrorType.Validation);
}