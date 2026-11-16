using DonationHub.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace DonationHub.Application.Features.Authentication.Register
{
    internal class RegisterHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterHandler(IIdentityService identityService, IValidator<RegisterCommand> validator)
        {
            _identityService = identityService;
            _validator = validator;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // should return result 
                throw new ValidationException(validationResult.Errors);
            }
            return await _identityService.CreateUserAsync(request);


        }
    }
}
