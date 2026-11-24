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
        private readonly SeedAdminOptions _adminOptions;
        private readonly SeedDonorOptions _donorOptions;
        private readonly SeedAdminOptions _managerOptions;
        private readonly DonationDBContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentitySeeder> _logger;

        public IdentitySeeder(IOptions<SeedAdminOptions> options, DonationDBContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, ILogger<IdentitySeeder> logger, IOptions<SeedDonorOptions> donoroptions, IOptions<SeedAdminOptions> manageroptions)
        {
            _adminOptions = options.Value;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _donorOptions = donoroptions.Value;
            _managerOptions = manageroptions.Value;
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
                    var adminUser = ApplicationUser.Create(_adminOptions.FullName, _adminOptions.Email, _adminOptions.Email);
                    var result = await _userManager.CreateAsync(adminUser, _adminOptions.Password);
                    if (result.Succeeded)
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    else
                        _logger.LogWarning("Could not seed default admin user: {Errors}",
                            string.Join("; ", result.Errors.Select(e => e.Description)));


                    // campaign manager
                    var managerUser = ApplicationUser.Create(_managerOptions.FullName, _managerOptions.Email, _managerOptions.Email);

                    var managerResult = await _userManager.CreateAsync(managerUser, _managerOptions.Password);
                    if (managerResult.Succeeded)
                        await _userManager.AddToRoleAsync(managerUser, "CampaignManager");
                    else
                        _logger.LogWarning("Could not seed default campaign manager user: {Errors}",
                            string.Join("; ", managerResult.Errors.Select(e => e.Description)));


                    // Donor
                    var donorUser = ApplicationUser.Create(_donorOptions.FullName, _donorOptions.Email, _donorOptions.Email);

                    var donorResult = await _userManager.CreateAsync(donorUser, _donorOptions.Password);
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
