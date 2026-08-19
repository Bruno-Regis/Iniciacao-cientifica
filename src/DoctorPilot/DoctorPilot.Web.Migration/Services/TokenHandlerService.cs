using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace DoctorPilot.Web.Migration.Services;

public sealed class TokenHandlerService(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context?.User.Identity?.IsAuthenticated is true)
        {
            var token = await context.GetTokenAsync("access_token");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
