using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public record RegisterCommand : IRequest<Result>
    {
        public RegisterCommand(string fullName, string email, string password, string userName, string preferredLanguage = "en")
        {
            FullName = fullName;
            Email = email;
            Password = password;
            UserName = userName;
            PreferredLanguage = preferredLanguage;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
