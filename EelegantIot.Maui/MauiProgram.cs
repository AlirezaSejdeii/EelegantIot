using Blazored.LocalStorage;
using EelegantIot.Shared;
using EelegantIot.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace EelegantIot.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddDependencies();
        builder.Services.AddMudServices();
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}