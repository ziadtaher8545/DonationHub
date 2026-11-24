using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DonationHub.Application.Features.Authentication.GetUser
{
    internal class GetUserHandler : IRequestHandler<GetUserQuery, Result<UserDataDto>>
    {
        private readonly IIdentityService _identityService;

        public GetUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }



        public async Task<Result<UserDataDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByEmailAsync(request.email, cancellationToken);
            return user;


        }
    }
}
