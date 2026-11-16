using DonationHub.Infrastructure.Common.Interfaces;

namespace DonationHub.Api.Extension
{
    public static class WebExtensions
    {
        public static async Task<WebApplication> IntializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Data");
            await seeder.SeedDataAsync();

            var identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await identityseeder.SeedDataAsync();

            return app;
        }
    }
}
