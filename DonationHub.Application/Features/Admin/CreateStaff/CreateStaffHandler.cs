using DonationHub.Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DonationHub.Application.Features.Admin.CreateStaff
{
    internal class CreateStaffHandler : IRequestHandler<CreateStaffCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<CreateStaffCommand> _validator;

        public CreateStaffHandler(IIdentityService identityService, IValidator<CreateStaffCommand> validator)
        {
            _identityService = identityService;
            _validator = validator;
        }

        public async Task<bool> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                // should result 
                throw new ValidationException(validationResult.Errors);
            }

            var result = await _identityService!.CreateRoleForUser(request.Email, request.Role, cancellationToken);
            return result;
        }
    }
}
