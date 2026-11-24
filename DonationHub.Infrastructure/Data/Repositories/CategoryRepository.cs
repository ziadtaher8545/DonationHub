using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Data.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {


        public CategoryRepository(DonationDBContext context) : base(context)
        {
        }


        public Result Delete(Category entity, CancellationToken ct = default)
        {
            var result = _context.Set<Category>().Remove(entity);
            if (result == null) return ValidationErrors.Failed;
            return Result.Ok();

        }


        public async Task<bool> HasCampaignsAsync(Guid id, CancellationToken ct)
        {
            var hasCampaigns = await _context.Campaigns.IgnoreQueryFilters().AnyAsync(c => c.CategoryId == id);

            return hasCampaigns;
        }
    }
}
