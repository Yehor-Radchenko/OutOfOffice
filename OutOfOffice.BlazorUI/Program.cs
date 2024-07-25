using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OutOfOffice.BlazorUI;
using OutOfOffice.BlazorUI.Services;
using OutOfOffice.BlazorUI.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:44371/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient("UnauthenticatedClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44371/");
});

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();