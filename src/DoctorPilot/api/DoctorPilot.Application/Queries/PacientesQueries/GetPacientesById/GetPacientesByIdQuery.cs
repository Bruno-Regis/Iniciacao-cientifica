using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById
{
    public class GetPacienteByIdQuery : IQuery<ResultViewModel<PacienteDetailsDto>>
    {
        public GetPacienteByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}