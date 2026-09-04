using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById
{
    public class GetAnamneseByIdHandler : IQueryHandler<GetAnamneseByIdQuery, ResultViewModel<AnamneseDetailsDto>>
    {
        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAnamneseByIdHandler(IAnamneseRepository anamneseRepository, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _anamneseRepository = anamneseRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<AnamneseDetailsDto>> Handle(GetAnamneseByIdQuery query, CancellationToken cancellationToken)
        {
            var anamnese = await _anamneseRepository.GetByIdComSessoesAsync(query.Id, cancellationToken);

            if (anamnese is null)
                return ResultViewModel<AnamneseDetailsDto>.Error("Anamnese não encontrada.");

            var paciente = await _pacienteRepository.GetByIdAsync(anamnese.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<AnamneseDetailsDto>.Error("Anamnese não encontrada.");

            return ResultViewModel<AnamneseDetailsDto>.Success(anamnese.ToDetailsDto());
        }
    }
}
