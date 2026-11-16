using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.CreateCategory
{
    internal class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {

        private readonly IGenericRepsitory<Domain.Entities.Category> _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryCommand> _validator;

        public CreateCategoryHandler(IGenericRepsitory<Domain.Entities.Category> categoryRepo, IUnitOfWork unitOfWork, IValidator<CreateCategoryCommand> validator)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // return result 
                throw new ValidationException(validationResult.Errors);
            }

            var category = Category.Create(request.Name, request.Description);
            try
            {

                await _categoryRepo.CreateAsync(category, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                //should be result
                throw new InvalidOperationException($"{ex.Message}");

            }
            return category.Id;





        }
    }
}
