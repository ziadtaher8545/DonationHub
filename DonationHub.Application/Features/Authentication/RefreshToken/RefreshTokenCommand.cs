using DonationHub.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<UserDto>
    {
    }
}
