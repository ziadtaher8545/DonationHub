using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Register
{
    public record RegisterCommand(string FullName, string Email, string Password, string UserName, string PreferredLanguage = "en") : IRequest<bool>
    {
    }
}
