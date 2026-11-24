using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    internal class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryToUpdateDto>>
    {
        private readonly IValidator<UpdateCategoryCommand> _validator;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(IValidator<UpdateCategoryCommand> validator, IGenericRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryToUpdateDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request);
            //if (!validationResult.IsValid)
            //{
            //    // shoud return result
            //    throw new ValidationException(validationResult.Errors);
            //}
            var category = Category.Create(request.Id, request.Name, request.Description);
            var resultUpdate = await _categoryRepository.UpdateAsync(category);
            if (!resultUpdate.IsSuccess)
                return resultUpdate.Error!;

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
                return ValidationErrors.Failed;
            return new CategoryToUpdateDto()
            {
                Description = category.Description,
                Name = category.Name,

            };




        }
    }
}
