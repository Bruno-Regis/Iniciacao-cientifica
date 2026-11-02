using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace DoctorPilot.Web.Migration.Endpoints;

public static class TranscriptionUploadEndpoint
{
    private const string AudioFieldName = "audio";

    public static IEndpointRouteBuilder MapTranscriptionUploadEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/api/transcription-upload/{agendamentoId:guid}",
                HandleAsync)
            .RequireAuthorization();

        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        Guid agendamentoId,
        HttpContext context,
        IAntiforgery antiforgery,
        IHttpClientFactory httpClientFactory,
        CancellationToken cancellationToken)
    {
        await antiforgery.ValidateRequestAsync(context);

        var accessToken = await context.GetTokenAsync("access_token");
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return Results.Json(
                new { message = "A sessao expirou. Entre novamente para transcrever a gravacao." },
                statusCode: StatusCodes.Status401Unauthorized);
        }

        if (!TryGetBoundary(context.Request.ContentType, out var boundary))
        {
            return Results.BadRequest(new { message = "Envie o audio usando multipart/form-data." });
        }

        var reader = new MultipartReader(boundary, context.Request.Body);
        MultipartSection? section;

        while ((section = await reader.ReadNextSectionAsync(cancellationToken)) is not null)
        {
            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                continue;

            var fieldName = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
            var suppliedFileName = HeaderUtilities.RemoveQuotes(
                contentDisposition.FileNameStar.HasValue
                    ? contentDisposition.FileNameStar
                    : contentDisposition.FileName).Value;

            if (!string.Equals(fieldName, AudioFieldName, StringComparison.Ordinal) ||
                string.IsNullOrWhiteSpace(suppliedFileName))
            {
                continue;
            }

            var fileName = GetSafeFileName(suppliedFileName, section.ContentType);
            using var audioContent = new StreamContent(section.Body);

            if (System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(section.ContentType, out var audioMediaType))
                audioContent.Headers.ContentType = audioMediaType;

            using var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(audioContent, AudioFieldName, fileName);

            using var apiRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/transcricoes/agendamentos/{agendamentoId}")
            {
                Content = multipartContent
            };
            apiRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                accessToken);

            var apiClient = httpClientFactory.CreateClient("DoctorPilotApi");
            using var apiResponse = await apiClient.SendAsync(
                apiRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            context.Response.StatusCode = (int)apiResponse.StatusCode;
            if (apiResponse.Content.Headers.ContentType is not null)
                context.Response.ContentType = apiResponse.Content.Headers.ContentType.ToString();

            await apiResponse.Content.CopyToAsync(context.Response.Body, cancellationToken);
            return Results.Empty;
        }

        return Results.BadRequest(new { message = "O campo de audio nao foi encontrado no upload." });
    }

    private static bool TryGetBoundary(string? contentType, out string boundary)
    {
        boundary = string.Empty;

        if (!MediaTypeHeaderValue.TryParse(contentType, out var mediaType) ||
            !string.Equals(
                mediaType.MediaType.Value,
                "multipart/form-data",
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        boundary = HeaderUtilities.RemoveQuotes(mediaType.Boundary).Value ?? string.Empty;
        return !string.IsNullOrWhiteSpace(boundary);
    }

    private static string GetSafeFileName(string suppliedFileName, string? contentType)
    {
        var fileName = Path.GetFileName(suppliedFileName);
        if (!string.IsNullOrWhiteSpace(fileName) && Path.HasExtension(fileName))
            return fileName;

        var extension = contentType?.Split(';', 2)[0].Trim().ToLowerInvariant() switch
        {
            "audio/ogg" => ".ogg",
            "audio/mp4" => ".m4a",
            "audio/mpeg" => ".mp3",
            "audio/wav" or "audio/x-wav" => ".wav",
            _ => ".webm"
        };

        var baseName = string.IsNullOrWhiteSpace(fileName)
            ? "consulta"
            : Path.GetFileNameWithoutExtension(fileName);

        return $"{baseName}{extension}";
    }
}
