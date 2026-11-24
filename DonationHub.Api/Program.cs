using DonationHub.Api.Exceptions;
using DonationHub.Api.Extension;
using DonationHub.Application.DependcyInjection;
using DonationHub.Domain.ValueObjects;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace DonationHub.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();

        // make Scallar to can add Authentication for this 
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Security =
                            [
                                new OpenApiSecurityRequirement
                                {
                                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                                }
                            ];
                document.Components.SecuritySchemes =
                    new Dictionary<string, IOpenApiSecurityScheme>
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "Enter your JWT access token"
                        }
                    };

                return Task.CompletedTask;
            });
        });
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();



        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            await app.IntializeDatabaseAsync();
            app.MapOpenApi();
            app.MapScalarApiReference();
        }


        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();





    }
}
