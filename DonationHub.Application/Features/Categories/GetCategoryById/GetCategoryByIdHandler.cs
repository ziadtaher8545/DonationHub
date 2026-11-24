using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.GetCategoryById
{
    internal class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly IDonationDbContext _context;
        public GetCategoryByIdHandler(IDonationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync(cancellationToken);
            var category = categories.FirstOrDefault(c => c.Id == request.Id);
            if (category == null) return CategoryErrors.NotFound;
            return new CategoryDto() { Id = category.Id, Name = category.Name, Description = category.Description };
        }
    }
}
