using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    internal class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryToUpdateDto>
    {
        private readonly IValidator<UpdateCategoryCommand> _validator;
        private readonly IGenericRepsitory<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(IValidator<UpdateCategoryCommand> validator, IGenericRepsitory<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryToUpdateDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                // shoud return result
                throw new ValidationException(validationResult.Errors);
            }
            var category = Category.Create(request.Id, request.Name, request.Description);
            await _categoryRepository.UpdateAsync(category);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
                return null;
            return new CategoryToUpdateDto()
            {
                Description = category.Description,
                Name = category.Name,

            };




        }
    }
}
