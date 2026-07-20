using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class AnamneseTemplateRepository : Repository<AnamneseTemplate>, IAnamneseTemplateRepository
    {
        public AnamneseTemplateRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<AnamneseTemplate>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<AnamneseTemplate?> GetByIdComSessoesAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
