using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.UpdateProfile
{
    internal class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserDataDto>
    {
        private readonly IIdentityService _identityService;

        public UpdateProfileHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDataDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateProfileAsync(request.Email, request.FullName, request.PreferredLanguage, cancellationToken);
            if (result is null) throw new UnauthorizedAccessException();
            return result;

        }
    }
}
