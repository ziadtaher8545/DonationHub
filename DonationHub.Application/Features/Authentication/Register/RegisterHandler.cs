using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using FluentValidation;
using MediatR;

namespace DonationHub.Application.Features.Authentication.Register
{
    internal class RegisterHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterHandler(IIdentityService identityService, IValidator<RegisterCommand> validator)
        {
            _identityService = identityService;
            _validator = validator;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    // should return result 
            //    return AuthErrors.InvalidCredentials;
            //}
            return await _identityService.CreateUserAsync(request);


        }
    }
}
