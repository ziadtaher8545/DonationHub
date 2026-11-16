using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Common.Interfaces
{
    public interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken ct = default);
    }
}
