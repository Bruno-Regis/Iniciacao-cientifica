using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Alergias;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente
{
    public class GetAllAlergiasByPacienteHandler : IQueryHandler<GetAllAlergiasByPacienteQuery, ResultViewModel<IEnumerable<AlergiaViewModel>>>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public GetAllAlergiasByPacienteHandler(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AlergiaViewModel>>> Handle(GetAllAlergiasByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdComAlergias(query.PacienteId);

            if (paciente is null)
                return ResultViewModel<IEnumerable<AlergiaViewModel>>.Error("Paciente não encontrado.");

            var viewModels = paciente.Alergias?.Select(AlergiaViewModel.FromEntity) ?? [];
            return ResultViewModel<IEnumerable<AlergiaViewModel>>.Success(viewModels);
        }
    }
}
