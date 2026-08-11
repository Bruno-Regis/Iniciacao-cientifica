
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Services
{
    public interface ITokenGenerator
    {
        (string Token, DateTime ExpiresAt) GenerateToken(Usuario usuario);
    }
}
