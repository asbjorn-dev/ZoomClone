using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using NetcodeHub.Packages.Extensions.LocalStorage;
using Twilio.Rest.Assistants.V1.Assistant;
using ZoomCloneApp.Client;
using ZoomCloneApp.Client.Extensions;
using ZoomCloneApp.Client.Interfaces;
using ZoomCloneApp.Client.Services;
using ZoomCloneApp.Client.States;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<NavState>();

// HttpClient configuration
// recieves configuration from appsettings.json so it can connect to the correct API address within the blazor app
string clientName = builder.Configuration["HttpClient:Name"]! ?? throw new InvalidOperationException("Config for HttpClient name is not configured");
builder.Services.AddHttpClient(clientName, options =>
{
    string baseAddress = builder.Configuration["HttpClient:BaseAddress"]! ??
                         throw new InvalidOperationException("Config for BaseAddress is not configured");
    // sets the base address for all HTTP requests made with this client
    options.BaseAddress = new Uri($"{baseAddress}/api/");
});

// Fluent
builder.Services.AddFluentUIComponents();

// Authentication services (I override the default AuthenticationStateProvider with a custom one)
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddAuthorizationCore();
// I use [authorize] attribute in pages so i need AddCascadingAuthenticationState() so AuthenticationStateProvider is available in all components
builder.Services.AddCascadingAuthenticationState();

// Local storage service 
builder.Services.AddNetcodeHubLocalStorageService();

// services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHttpExtension, HttpExtension>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddScoped<ITwilioService, TwilioService>();

await builder.Build().RunAsync();
