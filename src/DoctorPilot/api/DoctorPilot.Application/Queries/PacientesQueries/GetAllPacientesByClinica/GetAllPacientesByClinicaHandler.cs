using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica
{
    public class GetAllPacientesByClinicaHandler : IQueryHandler<GetAllPacientesByClinicaQuery, ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllPacientesByClinicaHandler(IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<PacienteItemDto>>> Handle(GetAllPacientesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.ClinicaId; // If the user is a super admin, use the provided ClinicaId; otherwise, use the ClinicaId from the current user context.

            var pacientes = await _pacienteRepository.GetByClinicaIdAsync(clinicaId, query.Page, query.PageSize);
            var viewModels = pacientes.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<PacienteItemDto>>.Success(viewModels);
        }
    }
}
