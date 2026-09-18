using DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo;
using Mediator;
using System.ComponentModel;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant.Tools
{
    public class GetAgendamentosTool
    {
        private readonly IMediator _mediator;

        public GetAgendamentosTool(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Description("Busca agendamentos de um profissional em um determinado período. Use esta ferramenta quando precisar obter os agendamentos de um profissional entre uma data/hora inicial e final.")]
        public async Task<object> GetAgendamentosAsync(
            [Description("Identificador do profissional.")]
            Guid profissionalId,

            [Description("Data e hora inicial do período que deve ser consultado.")]
            DateTime inicio,

            [Description("Data e hora final do período que deve ser consultado.")]
            DateTime fim,

            CancellationToken cancellationToken = default)
        {
            var query = new GetAllAgendamentosByPeriodoQuery(inicio, fim, profissionalId);
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
