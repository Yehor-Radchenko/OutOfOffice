using Serilog;
using OutOfOffice.API.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy => policy
            .WithOrigins("https://localhost:7109/")
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

await DataSeeder.SeedDataAsync(app);

app.ConfigureMiddleware();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
        response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        response.ContentType = "application/json";
        var problem = new ProblemDetails
        {
            Status = response.StatusCode,
            Instance = context.HttpContext.Request.Path,
            Detail = response.StatusCode == (int)HttpStatusCode.Unauthorized
                ? "Unauthorized access"
                : "Forbidden access"
        };
        await response.WriteAsync(JsonConvert.SerializeObject(problem));
    }
});

app.UseCors("AllowBlazorClient");

app.Run();
