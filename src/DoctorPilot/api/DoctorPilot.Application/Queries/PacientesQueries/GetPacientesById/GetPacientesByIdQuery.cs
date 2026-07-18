using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetPacienteById
{
    public class GetPacienteByIdQuery : IQuery<ResultViewModel<PacienteDetailsViewModel>>
    {
        public GetPacienteByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}