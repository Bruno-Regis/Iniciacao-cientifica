using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes
{
    public class SearchPacientesHandler : IQueryHandler<SearchPacientesQuery, ResultViewModel<IEnumerable<PacienteItemViewModel>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public SearchPacientesHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemViewModel>>> Handle(SearchPacientesQuery query, CancellationToken cancellationToken)
        {
            var pacientes = await _pacienteRepository.SearchAsync(query.Termo, query.Page, query.PageSize);
            var viewModels = pacientes.Select(PacienteItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<PacienteItemViewModel>>.Success(viewModels);
        }
    }
}
