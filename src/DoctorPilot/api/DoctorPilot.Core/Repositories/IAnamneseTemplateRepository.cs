using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Repositories
{
    public interface IAnamneseTemplateRepository : IRepository<AnamneseTemplate>
    {
        Task<IEnumerable<AnamneseTemplate>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<AnamneseTemplate?> GetByIdComSessoesAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
