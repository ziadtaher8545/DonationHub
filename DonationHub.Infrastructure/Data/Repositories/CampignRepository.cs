using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Repositories
{
    internal class CampignRepository : GenericRepository<Campaign>, ICampaignRepository
    {
        public CampignRepository(DonationDBContext context) : base(context)
        {

        }

        public Result Delete(Campaign entity, CancellationToken ct = default)
        {
            var result = _context.Set<Campaign>().Remove(entity);
            if (result == null)
                return CampaignErrors.NotFound;
            return Result.Ok();
        }

        public Result MakeCheckRowVersion(Campaign campaign, string rowVersion)
        {
            byte[] originalRowVersion;

            try
            {
                originalRowVersion = Convert.FromBase64String(rowVersion);
            }
            catch (FormatException)
            {
                return CampaignErrors.ConcurrencyConflict;
            }

            if (originalRowVersion.Length != 8)
                return CampaignErrors.ConcurrencyConflict;

            _context.Entry(campaign)
                .Property(c => c.RowVersion)
                .OriginalValue = originalRowVersion;
            return Result.Ok();
        }


    }
}
