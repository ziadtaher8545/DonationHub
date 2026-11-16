using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Application.Common.Options
{
    public class JwtOptions
    {
        [Required]
        public string SecretKey { get; set; } = default!;

        [Required]
        public string Issuer { get; set; } = default!;

        [Required]
        public string Audience { get; set; } = default!;

        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; }

        [Range(1, 365)]
        public int RefreshTokenExpiryDays { get; set; }
    }
}
