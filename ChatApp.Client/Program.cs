using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChatApp.Client.Handlers;
using ChatApp.Client.Mappers;
using ChatApp.Client.Providers;
using ChatApp.Client.Services;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

namespace ChatApp.Client;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("AnonymousClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7126");
        });

        builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

        builder.Services.AddHttpClient("AuthenticatedClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7126");
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

        builder.Services.AddMudServices();

        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped<ApiServices>();

        builder.Services.AddScoped<AuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
            provider.GetRequiredService<AuthStateProvider>());

        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddAutoMapper(typeof(BaseProfile));

        builder.Services.AddFluxor(config =>
        {
            config.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
            config.UseReduxDevTools();
#endif
        });

        await builder.Build().RunAsync();
    }
}