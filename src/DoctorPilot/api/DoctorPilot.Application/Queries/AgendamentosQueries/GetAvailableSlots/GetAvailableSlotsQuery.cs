using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAvailableSlots
{
    public class GetAvailableSlotsQuery : IQuery<ResultViewModel<IEnumerable<AvailableSlotDto>>>
    {
        public GetAvailableSlotsQuery(Guid profissionalId, DateTime inicio, DateTime fim, int duracaoMinutos)
        {
            ProfissionalId = profissionalId;
            Inicio = inicio;
            Fim = fim;
            DuracaoMinutos = duracaoMinutos;
        }

        public Guid ProfissionalId { get; }
        public DateTime Inicio { get; }
        public DateTime Fim { get; }
        public int DuracaoMinutos { get; }
    }
}
