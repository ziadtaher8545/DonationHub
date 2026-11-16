using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.GetCategoryById
{
    internal class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IGenericRepsitory<Domain.Entities.Category> _categoryRepository;
        public GetCategoryByIdHandler(IGenericRepsitory<Domain.Entities.Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null) return null;
            return new CategoryDto() { Id = category.Id, Name = category.Name, Description = category.Description };
        }
    }
}
