using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories;

public class ClinicaRepository : Repository<Clinica>, IClinicaRepository
{
    public ClinicaRepository(DoctorPilotDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsCnpjAsync(string cnpj)
        => _dbSet.AnyAsync(clinica => clinica.Cnpj == cnpj);

    public Task<bool> ExistsEmailAsync(string email)
        => _dbSet.AnyAsync(clinica => clinica.Email == email);
}
