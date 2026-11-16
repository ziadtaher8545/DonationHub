using DonationHub.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<UserDto>
    {
    }
}
