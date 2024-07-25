namespace OutOfOffice.API.Extentions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Extentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.OpenApi.Models;
using OutOfOffice.BLL.Services.Jwt;
using OutOfOffice.BLL.Services;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddDbContext<OutOfOfficeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.")));

        services.AddIdentity<Employee, IdentityRole<int>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<OutOfOfficeDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.CompletedTask;
                },
                OnRedirectToAccessDenied = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.CompletedTask;
                }
            };
        });


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddScoped<JwtService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<LeaveRequestService>();
        services.AddScoped<ApprovalRequestService>();
        services.AddScoped<ProjectService>();
        services.AddScoped<AbsenceReasonService>();
        services.AddScoped<SubdivisionService>();
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowBlazorClient");

        app.UseMiddleware<GlobalExceptionHandler>();
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}
