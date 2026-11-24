using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    internal class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteCategoryCommand> _validator;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }






        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            //var validationResult = await _validator.ValidateAsync(request);
            //if (!validationResult.IsValid)
            //    return CategoryErrors.NotFound;
            var categoryResult = await _categoryRepository.GetByIdAsync(request.Id, ct);


            if (!categoryResult.IsSuccess)
                return categoryResult.Error!;

            var category = categoryResult.Data;

            var hasCampaigns = await _categoryRepository.HasCampaignsAsync(request.Id, ct);

            if (hasCampaigns)
                return CategoryErrors.HasCampaigns;



            var deleteResult = _categoryRepository.Delete(category, ct);
            return deleteResult;

            //return Result.Ok();
        }
    }
}
