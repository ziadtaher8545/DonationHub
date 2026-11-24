using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(cc => cc.Name.NameAr)
                .NotEmpty().WithMessage("NameAr is required.")
                .MaximumLength(100).WithMessage("NameAr must not exceed 100 characters.");
            RuleFor(cc => cc.Name.NameEn)
              .NotEmpty().WithMessage("NameEn is required.")
              .MaximumLength(100).WithMessage("NameEn must not exceed 100 characters.");

            RuleFor(cc => cc.Description.DescriptionAr)
              .Empty()
              .MaximumLength(250).WithMessage("DescriptionAr must not exceed 250 characters.");
            RuleFor(cc => cc.Description.DescriptionEn)
              .Empty()
              .MaximumLength(250).WithMessage("DescriptionEn must not exceed 250 characters.");

        }
    }
}
