using DonationHub.Application.Features.Campaigns.CreateCampaign;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.UpdateCampaign
{


    public class UpdateCampaignValidator : AbstractValidator<UpdateCampaignCommand>
    {
        public UpdateCampaignValidator(TimeProvider timeProvider)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.TitleEn)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Title (En) Must Be At Most 200 chars.")
           ;

            RuleFor(x => x.TitleAr)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Title (Ar) Must Be At Most 200 chars.");


            RuleFor(x => x.DescriptionEn)
                .Empty()
                .MaximumLength(2000)
                .WithMessage("Description (En) Must Be At Most 2000 chars.");

            RuleFor(x => x.DescriptionAr)
                .Empty()
                .MaximumLength(2000)
                .WithMessage("Description (Ar) Must Be At Most 2000 chars.");


            RuleFor(x => x.GoalAmount)
                .GreaterThan(0).WithMessage("GoalAmount Must Be Greater Than 0.");


            RuleFor(x => x.EndDate)
                .Must(endDate => endDate > timeProvider.GetUtcNow())
                .WithMessage("Campaign end date must be in the future.");

            RuleFor(x => x.RowVersion)
                .NotEmpty()
                .Must(BeValidRowVersion)
                .WithMessage("Invalid RowVersion format.");
        }

        private static bool BeValidRowVersion(string rowVersion)
        {
            try
            {
                return Convert.FromBase64String(rowVersion).Length == 8;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

}
