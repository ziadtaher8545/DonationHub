using DonationHub.Domain.Common.Errors;
using FluentValidation;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign;

public class CreateCamaignCommandValidator : AbstractValidator<CreateCampaignCommand>
{
    public CreateCamaignCommandValidator()
    {
        RuleFor(x => x.TitleEn)
            .NotEmpty()
            .WithMessage("Campaign title in English is required.")
            .MaximumLength(200);

        RuleFor(x => x.TitleAr)
            .NotEmpty()
            .WithMessage("Campaign title in Arabic is required.")
            .MaximumLength(200);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn));

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr));

        RuleFor(x => x.GoalAmount)
            .NotNull()
            .Must(x => x > 0)
      //.WithState();
      .WithMessage("Campaign goal amount must be greater than zero.");

        RuleFor(x => x.EndDate)
            .Must(endDate => endDate > DateTimeOffset.UtcNow)
        //.WithState(_ => CampaignErrors.EndDateInPast);
        .WithMessage("Campaign end date must be in the future.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
        //.WithState(_ => CategoryErrors.NotFound);

        .WithMessage("Category is required.");
    }
}
