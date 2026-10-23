namespace DoctorPilot.Contracts.Transcricoes
{
    public class TranscribeAppointmentResponse
    {
        public Guid TranscricaoId { get; set; }
        public string TextoTranscrito { get; set; } = string.Empty;
    }
}
