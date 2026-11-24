using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Results;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.UpdateProfile
{
    internal class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<UserDataDto>>
    {
        private readonly IIdentityService _identityService;

        public UpdateProfileHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<UserDataDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateProfileAsync(request.Email, request.FullName, request.PreferredLanguage, cancellationToken);
            //if (!result.IsSuccess) return result.Error!;
            return result;

        }
    }
}
