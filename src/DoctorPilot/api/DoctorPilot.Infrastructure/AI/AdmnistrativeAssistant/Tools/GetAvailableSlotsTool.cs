using DoctorPilot.Application.Queries.AgendamentosQueries.GetAvailableSlots;
using Mediator;
using System.ComponentModel;

namespace DoctorPilot.Infrastructure.AI.AdmnistrativeAssistant.Tools
{
    public class GetAvailableSlotsTool
    {
        private readonly IMediator _mediator;

        public GetAvailableSlotsTool(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Description(
            "Busca os horários disponíveis de um profissional em um determinado período " +
            "para uma duração de atendimento informada. " +
            "Use esta ferramenta quando o usuário quiser saber quando um profissional " +
            "possui disponibilidade para um atendimento.")]
        public async Task<object> GetAvailableSlotsAsync(
            [Description("Identificador do profissional.")]
            Guid profissionalId,
            [Description("Data e hora inicial do período em que deve ser buscada disponibilidade.")]
            DateTime inicio,
            [Description("Data e hora final do período em que deve ser buscada disponibilidade.")]
            DateTime fim,
            [Description("Duração necessária para o atendimento, em minutos.")]
            int duracaoMinutos,
            CancellationToken cancellationToken = default)
        {
            var query = new GetAvailableSlotsQuery(profissionalId,inicio,fim,duracaoMinutos);

            return await _mediator.Send(query, cancellationToken);
        }

    }
}
