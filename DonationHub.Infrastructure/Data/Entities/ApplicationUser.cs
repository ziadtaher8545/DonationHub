using DonationHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public string FullName { get; private set; } = default!;

        public string PreferredLanguage { get; private set; } = "en";

        public bool IsActive { get; private set; } = true;

        public DateTimeOffset CreatedAt { get; private set; }
        public ICollection<Donation> Donations { get; set; }
        public static ApplicationUser Create(string fullName, string email, string? userName = null, string preferredLanguage = "en")
        {
            return new ApplicationUser
            {
                FullName = fullName,
                CreatedAt = DateTimeOffset.UtcNow,
                Email = email,
                UserName = userName,
                PreferredLanguage = preferredLanguage,
            };
        }
        public void ChangeFullName(string fullName)
        {
            FullName = fullName;
        }
        public void ChangePreferredLanguage(string preferredLanguage)
        {
            PreferredLanguage = preferredLanguage;
        }
    }
}
