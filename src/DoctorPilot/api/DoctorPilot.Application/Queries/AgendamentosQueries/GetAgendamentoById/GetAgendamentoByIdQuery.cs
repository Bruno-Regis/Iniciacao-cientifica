using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentoById
{
    public class GetAgendamentoByIdQuery : IQuery<ResultViewModel<AgendamentoDetailsDto>>
    {
        public GetAgendamentoByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
