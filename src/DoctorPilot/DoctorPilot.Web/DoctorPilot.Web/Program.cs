using DoctorPilot.Web.Client.Pages;
using DoctorPilot.Web.Client.Services;
using DoctorPilot.Web.Components;
using MudBlazor.Services;

namespace DoctorPilot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddMudServices();

            builder.Services.AddHttpClient("DoctorPilotApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7142/");
            });

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("DoctorPilotApi"));
            builder.Services.AddScoped<DoctorPilotApiClient>(sp =>
                new DoctorPilotApiClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("DoctorPilotApi")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
