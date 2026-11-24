using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DonationHub.Domain.Common.Errors
{
    public sealed record Error(string Code, string Message, ErrorType Type);


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        Validation,
        Unauthorized,
        Forbidden,
        PayloadTooLarge,
        Conflict,
        NotFound,
        UnprocessableEntity,
        TooManyRequests,
        Failure
    }
}
