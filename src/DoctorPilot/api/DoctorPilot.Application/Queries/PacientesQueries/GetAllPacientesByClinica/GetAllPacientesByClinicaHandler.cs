using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica
{
    public class GetAllPacientesByClinicaHandler : IQueryHandler<GetAllPacientesByClinicaQuery, ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public GetAllPacientesByClinicaHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemDto>>> Handle(GetAllPacientesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var pacientes = await _pacienteRepository.GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);
            var viewModels = pacientes.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<PacienteItemDto>>.Success(viewModels);
        }
    }
}
