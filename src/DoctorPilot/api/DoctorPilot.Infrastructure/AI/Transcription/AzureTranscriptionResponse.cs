namespace DoctorPilot.Infrastructure.AI.Transcription
{
    internal sealed class AzureTranscriptionResponse
    {
        public List<AzureCombinedPhrase> CombinedPhrases { get; set; } = [];
        public List<AzurePhrase> Phrases { get; set; } = [];
    }

    internal sealed class AzureCombinedPhrase
    {
        public string Text { get; set; } = string.Empty;
    }

    internal sealed class AzurePhrase
    {
        public int Speaker { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
