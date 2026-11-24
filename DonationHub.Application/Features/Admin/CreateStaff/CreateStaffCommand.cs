using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Admin.CreateStaff
{
    public record CreateStaffCommand(string FullName, string Email, string Password, string UserName, string Role, string PreferredLanguage = "en") : IRequest<Result>
    {
    }
}
