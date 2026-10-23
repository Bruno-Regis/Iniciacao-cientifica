namespace DoctorPilot.Infrastructure.AI.Transcription
{
    public class AzureSpeechOptions
    {
        public const string SectionName = "AzureSpeech";
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
