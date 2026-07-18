using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Pacientes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById
{
public class GetPacienteByIdHandler : IQueryHandler<GetPacienteByIdQuery, ResultViewModel<PacienteDetailsViewModel>>
{
    private readonly IPacienteRepository _pacienteRepository;

    public GetPacienteByIdHandler(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async ValueTask<ResultViewModel<PacienteDetailsViewModel>> Handle(GetPacienteByIdQuery query, CancellationToken cancellationToken)
    {
        var paciente = await _pacienteRepository.GetByIdAsync(query.Id);

        if (paciente is null)
            return ResultViewModel<PacienteDetailsViewModel>.Error("Paciente não encontrado.");

        return ResultViewModel<PacienteDetailsViewModel>.Success(PacienteDetailsViewModel.FromEntity(paciente));
    }
}
}