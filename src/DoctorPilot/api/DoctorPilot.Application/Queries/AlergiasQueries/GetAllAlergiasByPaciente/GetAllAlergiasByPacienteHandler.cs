using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente
{
    public class GetAllAlergiasByPacienteHandler : IQueryHandler<GetAllAlergiasByPacienteQuery, ResultViewModel<IEnumerable<AlergiaDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public GetAllAlergiasByPacienteHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AlergiaDto>>> Handle(GetAllAlergiasByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdComAlergias(query.PacienteId);

            if (paciente is null)
                return ResultViewModel<IEnumerable<AlergiaDto>>.Error("Paciente não encontrado.");

            var viewModels = paciente.Alergias?.Select(a => a.ToDto()) ?? [];
            return ResultViewModel<IEnumerable<AlergiaDto>>.Success(viewModels);
        }
    }
}
