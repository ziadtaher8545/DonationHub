using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    internal class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(rf => rf.RefreshToken).NotEmpty().WithMessage("Refresh Token Mustn't Be Empty ");
        }
    }
}
