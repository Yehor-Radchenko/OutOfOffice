using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OutOfOffice.Common.Services.Jwt;

namespace OutOfOffice.Extentions
{
    public static class ApiExtentions
    {
        public static void AddApiAuthentification(
            this IServiceCollection services,
            IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
