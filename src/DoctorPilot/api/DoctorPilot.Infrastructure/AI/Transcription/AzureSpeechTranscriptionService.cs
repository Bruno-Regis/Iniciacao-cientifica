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
            if (string.IsNullOrWhiteSpace(_options.Endpoint))
            {
                throw new InvalidOperationException("Azure Speech endpoint is not configured.");
            }

            if (!Uri.TryCreate(_options.Endpoint, UriKind.Absolute, out var configuredEndpoint)
                || (configuredEndpoint.Scheme != Uri.UriSchemeHttps && configuredEndpoint.Scheme != Uri.UriSchemeHttp))
            {
                throw new InvalidOperationException("Azure Speech endpoint must be a valid HTTP or HTTPS URL.");
            }

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Azure Speech API key is not configured.");
            }

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

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Azure Speech transcription failed with status {(int)response.StatusCode} ({response.ReasonPhrase}). " +
                    $"Response: {GetResponseExcerpt(responseBody)}",
                    inner: null,
                    response.StatusCode);
            }

            AzureTranscriptionResponse? azureResult;

            try
            {
                azureResult = JsonSerializer.Deserialize<AzureTranscriptionResponse>(responseBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (JsonException exception)
            {
                throw new InvalidOperationException(
                    "Azure Speech returned a malformed transcription response.",
                    exception);
            }

            if (azureResult?.CombinedPhrases is null || azureResult.Phrases is null)
            {
                throw new InvalidOperationException(
                    "Azure Speech returned an incomplete transcription response.");
            }

            var fullText = string.Join(" ", azureResult.CombinedPhrases.Select(x => x.Text));

            var segments = azureResult.Phrases
                .Select(x => new TranscriptionSegment(
                    x.Speaker,
                    x.Text))
                .ToList();

            return new TranscriptionResult(fullText, segments);
        }

        private static string GetResponseExcerpt(string responseBody)
        {
            const int maximumLength = 2_000;

            if (string.IsNullOrWhiteSpace(responseBody))
            {
                return "<empty>";
            }

            return responseBody.Length <= maximumLength
                ? responseBody
                : responseBody[..maximumLength];
        }
    }
}
