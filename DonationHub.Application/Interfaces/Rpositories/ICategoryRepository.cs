using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.Rpositories
{
    public interface ICategoryRepository : IGenericRepository<Category>, IDeletableRepository<Category>
    {
        Task<bool> HasCampaignsAsync(Guid id, CancellationToken ct = default);
    }
}
