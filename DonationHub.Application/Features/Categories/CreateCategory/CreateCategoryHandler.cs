using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.CreateCategory
{
    internal class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result>
    {

        private readonly IGenericRepository<Domain.Entities.Category> _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryCommand> _validator;

        public CreateCategoryHandler(IGenericRepository<Category> categoryRepo, IUnitOfWork unitOfWork, IValidator<CreateCategoryCommand> validator)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    // return result 
            //    return CategoryErrors.NotFound;
            //}

            var category = Category.Create(request.Name, request.Description);


            await _categoryRepo.CreateAsync(category, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();





        }
    }
}
