using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DonationHub.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum CampaignStatus
    {
        Active, Completed, Expired, Cancelled
    }
}
