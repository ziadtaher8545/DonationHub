using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DonationHub.Application.Features.Authentication.GetUser
{
    internal class GetUserHandler : IRequestHandler<GetUserQuery, UserDataDto>
    {
        private readonly IIdentityService _identityService;

        public GetUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }



        public async Task<UserDataDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByEmailAsync(request.email, cancellationToken);
            return user;


        }
    }
}
