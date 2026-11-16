using DonationHub.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.GetUser
{
    public record GetUserQuery(string email) : IRequest<UserDataDto>
    {
    }
}
