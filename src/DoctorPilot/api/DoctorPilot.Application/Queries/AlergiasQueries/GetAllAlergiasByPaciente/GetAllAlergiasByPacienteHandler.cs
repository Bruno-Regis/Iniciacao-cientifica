using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente
{
    public class GetAllAlergiasByPacienteHandler : IQueryHandler<GetAllAlergiasByPacienteQuery, ResultViewModel<IEnumerable<AlergiaDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllAlergiasByPacienteHandler(IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AlergiaDto>>> Handle(GetAllAlergiasByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdComAlergias(query.PacienteId);

            if (paciente is null)
                throw new NotFoundException("Paciente não encontrado.");

            if (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<IEnumerable<AlergiaDto>>.Error("Paciente não encontrado.");

            var viewModels = paciente.Alergias?.Select(a => a.ToDto()) ?? [];
            return ResultViewModel<IEnumerable<AlergiaDto>>.Success(viewModels);
        }
    }
}
