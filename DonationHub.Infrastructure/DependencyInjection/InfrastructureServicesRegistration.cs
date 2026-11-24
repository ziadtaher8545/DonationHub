using DonationHub.Application.Common.Options;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Infrastructure.Common.Interfaces;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.AppDBContext;
using DonationHub.Infrastructure.Data.Entities;
using DonationHub.Infrastructure.Data.Interceptors;
using DonationHub.Infrastructure.Data.Repositories;
using DonationHub.Infrastructure.Data.Seeders;
using DonationHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DonationHub.Infrastructure.DependencyInjection;

public static class InfrastructureServicesRegistration
{


    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {


        /// Options
        services.AddOptions<SeedAdminOptions>().BindConfiguration("SeedAdminOptions").ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<DonationOptions>().BindConfiguration("DonationOptions").ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<JwtOptions>().BindConfiguration("JwtOptions").ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<SeedDonorOptions>().BindConfiguration("SeedDonorOptions").ValidateDataAnnotations().ValidateOnStart();
        services.AddOptions<SeedManagerOptions>().BindConfiguration("SeedManagerOptions").ValidateDataAnnotations().ValidateOnStart();



        /// Identity
        services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole<Guid>>()
          .AddEntityFrameworkStores<DonationDBContext>().AddDefaultTokenProviders();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

        });

        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>() ??
            throw new InvalidOperationException("JwtOptions section is missing in the configuration.");

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(op =>
            {
                op.SaveToken = true;
                op.RequireHttpsMetadata = true;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

            });






        services.AddDbContext<DonationDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).AddInterceptors(new SoftDeletableInterceptor(TimeProvider.System));

        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IDeletableRepository<>), typeof(DeletableRepository<>));
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddKeyedScoped<IDataSeeder, IdentitySeeder>("Identity");
        services.AddKeyedScoped<IDataSeeder, DataSeeder>("Data");
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IDonationDbContext, DonationDBContext>();
        services.AddScoped<ICampaignRepository, CampignRepository>();






        return services;
    }
}
