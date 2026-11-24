using DonationHub.Application.Features.Admin.CreateStaff;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.RefreshToken;
using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Application.Features.Categories.CreateCategory;
using DonationHub.Application.Features.Categories.DeleteCategory;
using DonationHub.Application.Features.Categories.UpdateCategory;
using DonationHub.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DonationHub.Application.DependcyInjection;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {


        //services.AddScoped<IValidator<LoginCommand>, LoginValidator>();
        //services.AddScoped<IValidator<CreateStaffCommand>, CreateStaffValidator>();
        //services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryValidator>();
        //services.AddScoped<IValidator<RegisterCommand>, RegisterValidator>();
        //services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCateogryValidator>();

        //services.AddScoped<IValidator<DeleteCategoryCommand>, DeleteCategoryValidator>();
        //services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCateogryValidator>();
        //services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenValidator>();

        //services.AddValidatorsFromAssembly(typeof(ApplicationServicesRegistration).Assembly);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(op =>
        {
            op.RegisterServicesFromAssembly(typeof(ApplicationServicesRegistration).Assembly);
            op.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });

        return services;
    }
}
