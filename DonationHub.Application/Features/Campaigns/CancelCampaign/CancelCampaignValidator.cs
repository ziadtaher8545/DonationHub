using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.CancelCampaign
{
    public class CancelCampaignValidator : AbstractValidator<CancelCampaignCommand>
    {
        public CancelCampaignValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id Mustn't Be Empty.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Mustn't Be Empty.")
                .EmailAddress().WithMessage("Should Be Valid Email Address");
        }

    }
}
