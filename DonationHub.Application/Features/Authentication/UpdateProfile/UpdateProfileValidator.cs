using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.UpdateProfile
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileValidator()
        {


            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithErrorCode("User.FullNameRequired")
                .MaximumLength(100)
                .WithErrorCode("User.FullNameTooLong");

            RuleFor(x => x.PreferredLanguage)
                .NotEmpty()
                .WithErrorCode("User.PreferredLanguageRequired")
                .Must(language =>
                    language == "en" || language == "ar")
                .WithErrorCode("User.InvalidLanguage");
        }
    }
}
