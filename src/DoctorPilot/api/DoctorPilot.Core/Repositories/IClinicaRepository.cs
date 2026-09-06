using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories;

public interface IClinicaRepository : IRepository<Clinica>
{
    Task<bool> ExistsCnpjAsync(string cnpj);
    Task<bool> ExistsEmailAsync(string email);
}
