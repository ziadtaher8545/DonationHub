using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Application.Common.Options
{
    public class DonationOptions
    {
        [Required]
        public decimal MinAmount { get; set; }
        [Required]
        public decimal MaxAmount { get; set; }
        [Range(1, 1440)]
        public int PendingTimeoutMinutes { get; init; } = 30;

        [Required, StringLength(3, MinimumLength = 3)]
        public string DefaultCurrency { get; init; } = "USD";
    }
}
