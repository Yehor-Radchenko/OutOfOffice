using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OutOfOffice.BLL.Services.Jwt;
using System.Text;

namespace OutOfOffice.Extentions
{
    public static class ApiExtentions
    {
        public static void AddApiAuthentification(
            this IServiceCollection services,
            IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt-token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireHRManagerRole", policy => policy.RequireRole("HRManager"));
                options.AddPolicy("RequireProjectManagerRole", policy => policy.RequireRole("ProjectManager"));
                options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Employee"));
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
        }
    }
}
