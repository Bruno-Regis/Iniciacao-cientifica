using DoctorPilot.Api.Bootstrap;
using DoctorPilot.Api.ExceptionHandling;
using DoctorPilot.Api.Authorization;
using DoctorPilot.Application;
using DoctorPilot.Core.Enums;
using DoctorPilot.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace DoctorPilot.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.TenantRequired, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("ClinicaId");
                });

                options.AddPolicy(AuthorizationPolicies.ProfessionalRequired, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(nameof(RoleEnum.Medico));
                });

                options.AddPolicy(AuthorizationPolicies.OperationalAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(nameof(RoleEnum.Medico), nameof(RoleEnum.Admnistrativo));
                    policy.RequireClaim("ClinicaId");
                });

                options.AddPolicy(AuthorizationPolicies.AdministrativeAssistantAccess, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(nameof(RoleEnum.Admnistrativo));
                    policy.RequireClaim("ClinicaId");
                });
            });

            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DoctorPilotClient", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // builder.Services.AddOpenApi();

            var app = builder.Build();

            await SuperAdminBootstrap.EnsureCreatedAsync(app.Services);

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}
            app.UseCors("DoctorPilotClient");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
