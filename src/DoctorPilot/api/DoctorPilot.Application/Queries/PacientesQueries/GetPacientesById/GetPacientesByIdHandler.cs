using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById
{
public class GetPacienteByIdHandler : IQueryHandler<GetPacienteByIdQuery, ResultViewModel<PacienteDetailsDto>>
{
    private readonly IPacienteRepository _pacienteRepository;
    private readonly ICurrentUserService _currentUser;

    public GetPacienteByIdHandler(IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
    {
        _pacienteRepository = pacienteRepository;
        _currentUser = currentUser;
    }

    public async ValueTask<ResultViewModel<PacienteDetailsDto>> Handle(GetPacienteByIdQuery query, CancellationToken cancellationToken)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(query.Id);

        if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
            return ResultViewModel<PacienteDetailsDto>.Error("Paciente não encontrado.");

        return ResultViewModel<PacienteDetailsDto>.Success(paciente.ToDetailsDto());
    }
}
}
