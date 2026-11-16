using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Admin.CreateStaff
{
    internal class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
    {
        public CreateStaffValidator()
        {
            RuleFor(cs => cs.Email)
           .NotEmpty().WithMessage("Email is Required.")
           .EmailAddress().WithMessage("Invalid Email.");

            RuleFor(cs => cs.Role)
                .NotEmpty().WithMessage("Role is Required.")
                .Must(r => r is "Admin" or "CampaignManager");
            



        }
    }
}
