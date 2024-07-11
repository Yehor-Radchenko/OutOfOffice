using Serilog;
using OutOfOffice.API.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
