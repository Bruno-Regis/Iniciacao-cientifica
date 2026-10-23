using DoctorPilot.Contracts.Transcricoes;
using Mediator;

namespace DoctorPilot.Application.Commands.TranscricoesCommands.TranscribeAppointment
{
    public class TranscribeAppointmentCommand : IRequest<TranscribeAppointmentResponse>
    {
        public Guid AgendamentoId { get; set; }
        public Stream Audio { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
    }
}
