using DoctorPilot.Web.Migration.Components;
using DoctorPilot.Contracts.Auth;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Web.Migration.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using Radzen;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace DoctorPilot.Web.Migration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents(options =>
                {
                    options.DetailedErrors = true;
                });

            builder.Services.AddMudServices();
            builder.Services.AddRadzenComponents();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<TokenHandlerService>();

            // No modo Interactive Server, os componentes chamam a API diretamente.
            builder.Services.AddHttpClient("DoctorPilotApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7142/");
            }).AddHttpMessageHandler<TokenHandlerService>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("DoctorPilotApi"));
            builder.Services.AddScoped<Services.DoctorPilotApiClient>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "DoctorPilot.Auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/login";
                    options.SlidingExpiration = false;
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapPost("/account/login", async (
                [FromForm] LoginForm form,
                HttpContext context,
                IHttpClientFactory httpClientFactory,
                IConfiguration configuration) =>
            {
                var returnUrl = GetLocalReturnUrl(form.ReturnUrl);

                if (string.IsNullOrWhiteSpace(form.Email) || string.IsNullOrWhiteSpace(form.Senha))
                    return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl)}");

                HttpResponseMessage apiResponse;
                try
                {
                    var api = httpClientFactory.CreateClient("DoctorPilotApi");
                    apiResponse = await api.PostAsJsonAsync("api/auth/login", new { form.Email, form.Senha });
                }
                catch (HttpRequestException)
                {
                    return Results.Redirect($"/login?error=unavailable&returnUrl={Uri.EscapeDataString(returnUrl)}");
                }
                if (!apiResponse.IsSuccessStatusCode)
                    return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl)}");

                var result = await apiResponse.Content.ReadFromJsonAsync<ResultViewModel<LoginResponseDto>>();
                var jwt = result?.Data?.Token;
                if (string.IsNullOrWhiteSpace(jwt))
                    return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl)}");

                ClaimsPrincipal principal;
                try
                {
                    principal = new JwtSecurityTokenHandler().ValidateToken(jwt, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    }, out _);
                }
                catch (SecurityTokenException)
                {
                    return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl)}");
                }

                var identity = new ClaimsIdentity(principal.Claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Email, ClaimTypes.Role);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = result!.Data!.ExpiresAt
                };
                properties.StoreTokens([new AuthenticationToken { Name = "access_token", Value = jwt }]);

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                return Results.LocalRedirect(returnUrl);
            }).AllowAnonymous();

            app.MapPost("/account/logout", async (HttpContext context, IAntiforgery antiforgery) =>
            {
                await antiforgery.ValidateRequestAsync(context);
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.LocalRedirect("/login");
            })
                .RequireAuthorization();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }

        private static string GetLocalReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith('/') ||
                returnUrl.StartsWith("//") || returnUrl.StartsWith("/\\"))
                return "/";

            return returnUrl;
        }
    }

    public sealed class LoginForm
    {
        public string Email { get; init; } = string.Empty;
        public string Senha { get; init; } = string.Empty;
        public string? ReturnUrl { get; init; }
    }
}
