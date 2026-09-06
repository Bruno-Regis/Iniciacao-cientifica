using DoctorPilot.Application.Services;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;

namespace DoctorPilot.Api.Bootstrap;

public static class SuperAdminBootstrap
{
    public static async Task EnsureCreatedAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var usuarioRepository = services.GetRequiredService<IUsuarioRepository>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(SuperAdminBootstrap));

        if (await usuarioRepository.ExistsSuperAdminAsync(cancellationToken))
        {
            logger.LogInformation("SuperAdmin já existe.");
            return;
        }

        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Bootstrap do SuperAdmin não configurado.");

        var passwordHash = passwordHasher.Hash(password);
        var superAdmin = new Usuario(null, email.Trim(), passwordHash, RoleEnum.SuperAdmin);

        await usuarioRepository.AddAsync(superAdmin);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("SuperAdmin bootstrap concluído.");
    }
}
