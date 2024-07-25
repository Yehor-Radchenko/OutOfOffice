using Serilog;
using OutOfOffice.API.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OutOfOffice.Common.Services.Jwt;
using System.Text;

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

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireHRManagerRole", policy => policy.RequireRole("HRManager"));
    options.AddPolicy("RequireProjectManagerRole", policy => policy.RequireRole("ProjectManager"));
    options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Employee"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

await DataSeedExtention.SeedDataAsync(app);

app.ConfigureMiddleware();

app.Run();