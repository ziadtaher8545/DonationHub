using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DonationHub.Application.Features.Admin.CreateStaff
{
    internal class CreateStaffHandler : IRequestHandler<CreateStaffCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<CreateStaffCommand> _validator;

        public CreateStaffHandler(IIdentityService identityService, IValidator<CreateStaffCommand> validator)
        {
            _identityService = identityService;
            _validator = validator;
        }

        public async Task<Result> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            //if (!validationResult.IsValid)
            //{
            //    return AuthErrors.InvalidCredentials;
            //}

            var result = await _identityService!.CreateStaffAsync(request, cancellationToken);

            return result;
        }
    }
}
