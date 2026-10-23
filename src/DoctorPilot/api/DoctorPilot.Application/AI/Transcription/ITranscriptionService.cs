namespace DoctorPilot.Application.AI.Transcription
{
    public interface ITranscriptionService
    {
        Task<TranscriptionResult> TranscribeAsync(Stream audio, string fileName, CancellationToken cancellationToken = default);
    }
}
