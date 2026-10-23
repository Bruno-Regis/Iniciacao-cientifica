using DoctorPilot.Application.AI.AdmnistrativeAssistant;
using DoctorPilot.Application.AI.Transcription;
using DoctorPilot.Application.Services;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant;
using DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant.Tools;
using DoctorPilot.Infrastructure.AI.Transcription;
using DoctorPilot.Infrastructure.Persistence;
using DoctorPilot.Infrastructure.Repositories;
using DoctorPilot.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DoctorPilot.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddData(configuration)
                .AddRepositories()
                .AddAI(configuration)
                .AddAuth(configuration);

            return services;
        }
        private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DoctorPilotCs");
            services.AddDbContext<DoctorPilotDbContext>(o => o.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<IAnamneseRepository, AnamneseRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IClinicaRepository, ClinicaRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
            services.AddScoped<IAnamneseTemplateRepository, AnamneseTemplateRepository>();
            services.AddScoped<IEvolucaoRepository, EvolucaoRepository>();
            services.AddScoped<IHorarioProfissionalRepository, HorarioProfissionalRepository>();
            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])
                        )
                    };
                });

            services.AddAuthorization();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
        private static IServiceCollection AddAI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<GetProfissionaisByNomeTool>();
            services.AddScoped<GetAgendamentosTool>();
            services.AddScoped<GetAvailableSlotsTool>();
            services.AddSingleton<InMemoryAgentSessionStore>();
            services.AddScoped<IAdmnistrativeAssistantService, AdmnistrativeAssistantService>();
            services.Configure<AzureSpeechOptions>(configuration.GetSection(AzureSpeechOptions.SectionName));
            services.AddHttpClient<ITranscriptionService, AzureSpeechTranscriptionService>();

            return services;
        }
    }
}

