using Microsoft.AspNetCore.Identity;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using OutOfOffice.DAL.Seed;

namespace OutOfOffice.API.Extentions
{
    public static class DataSeedExtention
    {
        public static async Task SeedDataAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<OutOfOfficeDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

            await DataSeeder.SeedRolesAsync(roleManager);
            await DataSeeder.SeedPositionsAsync(dbContext);
            await DataSeeder.SeedEmployeesAsync(dbContext, userManager);
        }
    }
}
