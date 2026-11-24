using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using DonationHub.Infrastructure.Common.Interfaces;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Seeders
{
    internal class DataSeeder : IDataSeeder
    {
        private readonly DonationDBContext _context;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(DonationDBContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            var pendingMigration = await _context.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigration.Any())
                await _context.Database.MigrateAsync(ct);
            if (!await _context.Categories.AnyAsync(cancellationToken: ct))
            {




                // categories
                var categories = new[]
                   {
                    Category.Create(new Name  ("الصحة","Health")),
                    Category.Create(new Name("التعليم" ,"Education")),
                    Category.Create(new Name ( "الإغاثة", "Relief")),
                    Category.Create(new Name(    "الأيتام","Orphans")),
                    Category.Create(new  Name(  "البيئة","Environment" ))
                };

                await _context.AddRangeAsync(categories);
                var res = await _context.SaveChangesAsync(ct);
                if (res > 0)
                    _logger.LogInformation("Categories are Seeded Successfully");
            }


        }
    }
}
