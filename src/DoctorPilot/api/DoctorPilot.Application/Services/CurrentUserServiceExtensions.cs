namespace DoctorPilot.Application.Services;

public static class CurrentUserServiceExtensions
{
    public static Guid GetRequiredClinicaId(this ICurrentUserService currentUser)
    {
        return currentUser.ClinicaId
            ?? throw new UnauthorizedAccessException("Clínica não identificada para este usuário.");
    }
}
