using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using pdfTemplator.Client;
using pdfTemplator.Client.Services.Identity;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Client.Services.Preferences;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("pdfTemplator.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("pdfTemplator.ServerAPI"));

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IdentityAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<IdentityAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthorizeService, AuthorizeService>();

builder.Services.AddMudServices(configuration =>
{
    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
    configuration.SnackbarConfiguration.ShowCloseIcon = false;
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IFieldService, FieldService>();
builder.Services.AddScoped<IConversionService, ConversionService>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<IDataSourceService, DataSourceService>();
builder.Services.AddScoped<IAutomatedTemplateService, AutomatedTemplateService>();
builder.Services.AddScoped<ClientPreferenceManager>();

await builder.Build().RunAsync();
