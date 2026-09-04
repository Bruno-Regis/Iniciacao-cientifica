using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAllAnamnesesByPaciente
{
    public class GetAllAnamnesesByPacienteHandler : IQueryHandler<GetAllAnamnesesByPacienteQuery, ResultViewModel<IEnumerable<AnamneseItemDto>>>
    {

        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllAnamnesesByPacienteHandler(IAnamneseRepository anamneseRepository, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _anamneseRepository = anamneseRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AnamneseItemDto>>> Handle(GetAllAnamnesesByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(query.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<IEnumerable<AnamneseItemDto>>.Error("Paciente não encontrado.");

            var anamneses = await _anamneseRepository.GetByPacienteIdAsync(
                query.PacienteId,
                query.Page,
                query.PageSize,
                cancellationToken);
            var viewModels = anamneses.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AnamneseItemDto>>.Success(viewModels);
        }
    }
}
