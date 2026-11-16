using DonationHub.Application.Common.Options;
using DonationHub.Infrastructure.Common.Interfaces;
using DonationHub.Infrastructure.Data.AppDBContext;
using DonationHub.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Seeders
{
    internal class IdentitySeeder : IDataSeeder
    {
        private readonly IOptions<SeedAdminOptions> _options;
        private readonly DonationDBContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentitySeeder> _logger;

        public IdentitySeeder(IOptions<SeedAdminOptions> options, DonationDBContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, ILogger<IdentitySeeder> logger)
        {
            _options = options;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }



        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigration = await _context.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigration.Any())
                    await _context.Database.MigrateAsync(ct);


                if (!await _roleManager.Roles.AnyAsync(cancellationToken: ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole<Guid>("CampaignManager"));
                    await _roleManager.CreateAsync(new IdentityRole<Guid>("Donor"));
                    _logger.LogInformation("Roles are added Sucessfully");
                }
                if (!await _userManager.Users.AnyAsync(cancellationToken: ct))
                {
                    // admin
                    var adminOptions = _options.Value;
                    var adminUser = ApplicationUser.Create(adminOptions.FullName, adminOptions.Email, "ZiadTaher");
                    var result = await _userManager.CreateAsync(adminUser, adminOptions.Password);
                    if (result.Succeeded)
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    else
                        _logger.LogWarning("Could not seed default admin user: {Errors}",
                            string.Join("; ", result.Errors.Select(e => e.Description)));


                    // campaign manager
                    var managerUser = ApplicationUser.Create("Campaign Manager", "manager@gmail.com", "campaignmanager");

                    var managerResult = await _userManager.CreateAsync(managerUser, "P@ssw0rd");
                    if (managerResult.Succeeded)
                        await _userManager.AddToRoleAsync(managerUser, "CampaignManager");
                    else
                        _logger.LogWarning("Could not seed default campaign manager user: {Errors}",
                            string.Join("; ", managerResult.Errors.Select(e => e.Description)));


                    // Donor
                    var donorUser = ApplicationUser.Create("Donor User", "donor@gmail.com", "DonorUser");

                    var donorResult = await _userManager.CreateAsync(donorUser, "P@ssw0rd");
                    if (donorResult.Succeeded)
                        await _userManager.AddToRoleAsync(donorUser, "Donor");
                    else
                        _logger.LogWarning("Could not seed default donor user: {Errors}",
                            string.Join("; ", donorResult.Errors.Select(e => e.Description)));

                }




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Identity data seeding failed.");
            }


        }

    }
}
