using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Application.Features.Categories.GetAllCategories
{
    internal class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
    {

        private readonly IDonationDbContext _context;

        public GetAllCategoriesHandler(IDonationDbContext context)
        {

            _context = context;
        }


        public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }
    }
}
