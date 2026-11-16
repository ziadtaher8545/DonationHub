using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.ChangePassword
{



    internal class ChangePasswordCommandValidator
        : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithErrorCode("Auth.CurrentPasswordRequired");

            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage("Password is Required.")
                .MinimumLength(8).WithMessage("Password Must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .WithErrorCode("Auth.ConfirmPasswordRequired")
                .Equal(x => x.NewPassword)
                .WithErrorCode("Auth.PasswordMismatch");
        }
    }

}
