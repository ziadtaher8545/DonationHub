using DonationHub.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.ChangePassword
{
    internal class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IIdentityService _identityService;


        public ChangePasswordHandler(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                throw new ArgumentException("MisMatched Password");

            var userId = request.Email;

            var result = await _identityService.ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword, cancellationToken);
            return result;

        }
    }
}
