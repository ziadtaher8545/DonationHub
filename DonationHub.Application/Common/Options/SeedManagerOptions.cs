using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Application.Common.Options
{
    public class SeedManagerOptions
    {
        [Required, EmailAddress]
        public string Email { get; init; } = default!;

        [Required]
        public string Password { get; init; } = default!;

        [Required]
        public string FullName { get; init; } = default!;
    }
}
