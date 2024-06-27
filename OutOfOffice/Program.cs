using Microsoft.AspNetCore.Identity;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Services;
using OutOfOffice.Common.Services.Jwt;
using Microsoft.Extensions.Options;
using OutOfOffice.Extentions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using OutOfOffice.Common.Dto;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddDbContext<OutOfOfficeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddIdentity<Employee, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<OutOfOfficeDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddApiAuthentification(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<LeaveRequestService>();
builder.Services.AddScoped<ApprovalRequestService>();
builder.Services.AddScoped<ProjectService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<OutOfOfficeDbContext>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
    await SeedRolesAsync(roleManager);
    await SeedPositionsAsync(dbContext);
    await SeedAdminAsync(dbContext, userManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
{
    string[] roleNames = { "Admin", "Employee", "HRManager", "ProjectManager" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }
}

async Task SeedPositionsAsync(OutOfOfficeDbContext context)
{
    if (!context.Positions.Any())
    {
        string[] positionNames = { "Junior Developer", "Middle Developer", "HR Manager", "Project Manager", "Admin" };

        var positionsToAdd = positionNames.Select(name => new Position { Name = name }).ToList();

        await context.Positions.AddRangeAsync(positionsToAdd);
        await context.SaveChangesAsync();
    }
}

async Task SeedAdminAsync(OutOfOfficeDbContext context, UserManager<Employee> userManager)
{
    if (!context.Employees.Any()) {
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