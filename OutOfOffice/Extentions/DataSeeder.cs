using Microsoft.AspNetCore.Identity;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.API.Extentions
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var dbContext = serviceProvider.GetRequiredService<OutOfOfficeDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

            await SeedRolesAsync(roleManager);
            await SeedPositionsAsync(dbContext);
            await SeedAdminAsync(dbContext, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roleNames = { "Admin", "Employee", "HRManager", "ProjectManager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        private static async Task SeedPositionsAsync(OutOfOfficeDbContext context)
        {
            if (!context.Positions.Any())
            {
                string[] positionNames = { "Junior Developer", "Middle Developer", "HR Manager", "Project Manager", "Admin" };

                var positionsToAdd = positionNames.Select(name => new Position { Name = name }).ToList();

                await context.Positions.AddRangeAsync(positionsToAdd);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAdminAsync(OutOfOfficeDbContext context, UserManager<Employee> userManager)
        {
            if (!context.Employees.Any())
            {
                var employee = new Employee
                {
                    FullName = "Admin",
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    PositionId = 5,
                };

                await userManager.CreateAsync(employee, "1234qwer");
                await userManager.AddToRoleAsync(employee, "Admin");
            }
        }
    }
}
