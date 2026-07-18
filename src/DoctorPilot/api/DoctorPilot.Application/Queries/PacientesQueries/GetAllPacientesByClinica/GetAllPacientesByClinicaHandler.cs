using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica
{
    public class GetAllPacientesByClinicaHandler : IQueryHandler<GetAllPacientesByClinicaQuery, ResultViewModel<IEnumerable<PacienteItemViewModel>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public GetAllPacientesByClinicaHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemViewModel>>> Handle(GetAllPacientesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var pacientes = await _pacienteRepository.GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);
            var viewModels = pacientes.Select(PacienteItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<PacienteItemViewModel>>.Success(viewModels);
        }
    }
}
