using DonationHub.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.UpdateProfile
{
    public record UpdateProfileCommand(string Email, string FullName, string PreferredLanguage) : IRequest<UserDataDto>;
}
