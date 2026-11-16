using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace DonationHub.Application.Features.Authentication.Register
{
    internal class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {

            RuleFor(rg => rg.FullName)
                .NotEmpty().WithMessage("Full Name is Required.")
                .MaximumLength(50).WithMessage("Full name cannot exceed 50 characters.");


            RuleFor(rg => rg.Email)
                .NotEmpty().WithMessage("Email is Required.")
                .EmailAddress().WithMessage("Invalid Email.");

            RuleFor(rg => rg.Password)
                .NotEmpty().WithMessage("Password is Required.")
                .MinimumLength(8).WithMessage("Password Must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(rg => rg.PreferredLanguage)
                 .NotEmpty()
                .Must(language => language is "en" or "ar")
                .WithMessage("Preferred language must be either 'en' or 'ar'.");

        }
    }
}
