using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    internal class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteCategoryCommand> _validator;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IValidator<DeleteCategoryCommand> validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }






        public async Task<bool> Handle(
            DeleteCategoryCommand request,
            CancellationToken ct)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new InvalidOperationException();
            var category = await _categoryRepository.GetByIdAsync(request.Id, ct);

            if (category is null)
                throw new InvalidOperationException("Category.NotFound");

            var hasCampaigns = await _categoryRepository.HasCampaignsAsync(request.Id, ct);

            if (hasCampaigns)
                throw new InvalidOperationException("Category.HasCampaigns");

            _categoryRepository.Delete(category, ct);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result < 0)
                throw new InvalidOperationException("");

            return true;
        }
    }
}
