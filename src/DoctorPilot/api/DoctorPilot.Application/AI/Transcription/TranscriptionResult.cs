namespace DoctorPilot.Application.AI.Transcription
{
    public sealed record TranscriptionResult(string FullText, IReadOnlyList<TranscriptionSegment> Segments);

    public sealed record TranscriptionSegment(int Speaker, string Text);
}
