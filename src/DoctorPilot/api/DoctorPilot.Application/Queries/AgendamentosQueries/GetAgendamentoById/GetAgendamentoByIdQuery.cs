using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById
{
    public class GetAgendamentoByIdQuery : IQuery<ResultViewModel<AgendamentoDetailsViewModel>>
    {
        public GetAgendamentoByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
