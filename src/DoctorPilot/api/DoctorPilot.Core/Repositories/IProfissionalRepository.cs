using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Repositories
{
    public interface IProfissionalRepository : IRepository<Profissional>
    {
        Task<Profissional?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
        Task<Profissional?> GetByIdComRelacionamentosAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Profissional>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<IEnumerable<Profissional>> SearchByNomeAsync(Guid clinicaId, string nome, int limit = 10, CancellationToken cancellationToken = default);
    }
}
