using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes
{
    public class SearchPacientesHandler : IQueryHandler<SearchPacientesQuery, ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;
        public SearchPacientesHandler(IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemDto>>> Handle(SearchPacientesQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.IsSuperAdmin
                ? query.ClinicaId
                : _currentUser.GetRequiredClinicaId();

            var pacientes = await _pacienteRepository.SearchAsync(clinicaId, query.Termo, query.Page, query.PageSize);
            var viewModels = pacientes.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<PacienteItemDto>>.Success(viewModels);
        }
    }
}
