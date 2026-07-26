using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes
{
    public class SearchPacientesHandler : IQueryHandler<SearchPacientesQuery, ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public SearchPacientesHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemDto>>> Handle(SearchPacientesQuery query, CancellationToken cancellationToken)
        {
            var pacientes = await _pacienteRepository.SearchAsync(query.ClinicaId,query.Termo, query.Page, query.PageSize);
            var viewModels = pacientes.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<PacienteItemDto>>.Success(viewModels);
        }
    }
}
