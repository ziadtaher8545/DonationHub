using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {


        public CategoryRepository(DonationDBContext context) : base(context)
        {
        }


        public void Delete(Category entity, CancellationToken ct = default)
        {
            _context.Set<Category>().Remove(entity);

        }

        public async Task<bool> HasCampaignsAsync(Guid id, CancellationToken ct)
        {
            var category = await _context.Categories.Where(c => c.Id == id).Include(c => c.Campaigns).FirstOrDefaultAsync();
            if (category == null) return false;
            var campaigns = category.Campaigns;
            if (campaigns is null) return false;

            var hasCampaigns = campaigns.Count();
            return hasCampaigns > 0;
        }
    }
}
