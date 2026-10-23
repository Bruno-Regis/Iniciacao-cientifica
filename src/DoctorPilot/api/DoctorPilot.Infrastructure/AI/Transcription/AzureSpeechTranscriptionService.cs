using DoctorPilot.Application.AI.Transcription;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace DoctorPilot.Infrastructure.AI.Transcription
{
    public class AzureSpeechTranscriptionService : ITranscriptionService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureSpeechOptions _options;

        public AzureSpeechTranscriptionService(HttpClient httpClient, IOptions<AzureSpeechOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<TranscriptionResult> TranscribeAsync(Stream audio, string fileName, CancellationToken cancellationToken = default)
        {
            var endpoint = _options.Endpoint.TrimEnd('/');

            var requestUri =
                $"{endpoint}/speechtotext/transcriptions:transcribe?api-version=2025-10-15";

            using var content = new MultipartFormDataContent();

            var audioContent = new StreamContent(audio);

            content.Add(audioContent, "audio", fileName);

            var definition = new
            {
                locales = Array.Empty<string>(),
                diarization = new
                {
                    maxSpeakers = 2,
                    enabled = true
                }
            };

            var definitionJson = JsonSerializer.Serialize(definition);

            var definitionContent = new StringContent(definitionJson, Encoding.UTF8, "application/json");

            content.Add(definitionContent, "definition");

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            request.Headers.Add("Ocp-Apim-Subscription-Key", _options.ApiKey);

            request.Content = content;

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            response.EnsureSuccessStatusCode();

            var azureResult = JsonSerializer.Deserialize<AzureTranscriptionResponse>(responseBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (azureResult is null)
            {
                throw new InvalidOperationException(
                    "Azure Speech returned an invalid transcription response.");
            }

            var fullText = string.Join(" ", azureResult.CombinedPhrases.Select(x => x.Text));

            var segments = azureResult.Phrases
                .Select(x => new TranscriptionSegment(
                    x.Speaker,
                    x.Text))
                .ToList();

            return new TranscriptionResult(fullText, segments);
        }
    }
}
