using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.ChangePassword
{
    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IIdentityService _identityService;


        public ChangePasswordHandler(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            //if (request.NewPassword != request.ConfirmNewPassword)
            //    return AuthErrors.InvalidCredentials;

            var email = request.Email;

            var result = await _identityService.ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword, cancellationToken);
            return result;

        }
    }
}
