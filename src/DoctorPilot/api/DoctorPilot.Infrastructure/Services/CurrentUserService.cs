using DoctorPilot.Application.Services;
using Microsoft.AspNetCore.Http;
using DoctorPilot.Core.Enums;
using System.Security.Claims;

namespace DoctorPilot.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UsuarioId =>
            Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Usuário não autenticado."));

        public Guid ClinicaId =>
            Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue("ClinicaId")
                ?? throw new UnauthorizedAccessException("Clínica não identificada no token."));

        public string Role =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role)
                ?? throw new UnauthorizedAccessException("Role não identificada no token.");

        public bool IsSuperAdmin => Role == nameof(RoleEnum.SuperAdmin);
    }
}
