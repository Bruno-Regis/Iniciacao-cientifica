using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById
{
public class GetPacienteByIdHandler : IQueryHandler<GetPacienteByIdQuery, ResultViewModel<PacienteDetailsDto>>
{
    private readonly IPacienteRepository _pacienteRepository;

    public GetPacienteByIdHandler(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async ValueTask<ResultViewModel<PacienteDetailsDto>> Handle(GetPacienteByIdQuery query, CancellationToken cancellationToken)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(query.Id);

        if (paciente is null)
            return ResultViewModel<PacienteDetailsDto>.Error("Paciente não encontrado.");

        return ResultViewModel<PacienteDetailsDto>.Success(paciente.ToDetailsDto());
    }
}
}