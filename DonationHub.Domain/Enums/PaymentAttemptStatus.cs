using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DonationHub.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum PaymentAttemptStatus
    {
        Created, Succedded, Failed, Expired
    }
}
