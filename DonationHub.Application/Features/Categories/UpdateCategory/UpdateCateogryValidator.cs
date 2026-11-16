using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    internal class UpdateCateogryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCateogryValidator()
        {
            RuleFor(x => x.Name.NameAr)
                .NotEmpty().WithMessage("Name (Arabic) is required.")
                .MaximumLength(100).WithMessage("Name (Arabic) must not exceed 100 characters.");
            RuleFor(x => x.Name.NameEn)
                .NotEmpty().WithMessage("Name (English) is required.")
                .MaximumLength(100).WithMessage("Name (English) must not exceed 100 characters.");
            RuleFor(x => x.Description.DescriptionAr)
                .Empty()
                .MaximumLength(500).WithMessage("Description (Arabic) is required.");
            RuleFor(x => x.Description.DescriptionEn)
                .Empty()
                .MaximumLength(500).WithMessage("Description (English) is required.");

        }
    }
}
