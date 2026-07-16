using Microsoft.Extensions.DependencyInjection;
using Mediator;

namespace DoctorPilot.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddHandlers();
                //.AddValidation();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });

            return services;
        }

        //private static IServiceCollection AddValidation(this IServiceCollection services)
        //{
        //    services
        //        .AddFluentValidationAutoValidation()
        //        .AddValidatorsFromAssemblyContaining<ApplicationModule>();

        //    return services;
        //}
    }
}