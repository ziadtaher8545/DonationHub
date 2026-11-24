using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.DeleteCampaign
{
    public class DeleteCampaignValidator : AbstractValidator<DeleteCampaignCommand>
    {
        public DeleteCampaignValidator()
        {
            RuleFor(ca => ca.Id).NotEmpty().WithMessage("The Id Is Required.");
        }
    }
}
