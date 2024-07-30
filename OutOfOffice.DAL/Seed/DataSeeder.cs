using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Common.Enums;
using OutOfOffice.DAL.Context;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
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

        public static async Task SeedPositionsAsync(OutOfOfficeDbContext context)
        {
            if (!await context.Positions.AnyAsync())
            {
                string[] positionNames =
                {
                    "Junior Developer", "Middle Developer", "Senior Developer",
                    "HR Manager", "Project Manager", "Admin",
                    "QA Engineer", "DevOps Engineer", "Product Owner",
                    "UI/UX Designer"
                };

                var positionsToAdd = positionNames.Select(name => new Position { Name = name }).ToList();

                await context.Positions.AddRangeAsync(positionsToAdd);
                await context.SaveChangesAsync();
            }
        }

       

        public static async Task SeedEmployeesAsync(OutOfOfficeDbContext context, UserManager<Employee> userManager)
        {
            if (!await context.Employees.AnyAsync())
            {
                await SeedSubdivisionsAsync(context);
                await SeedAbsenceReasonsAsync(context);

                string[] imageFilePaths = Directory.GetFiles("../OutOfOffice.DAL/Seed/Images");

                var employees = new Employee[]
                {
                    new Employee { FullName = "Admin", Email = "admin@admin.com", UserName = "admin@admin.com", SubdivisionId = 2, PositionId = 6, Status = EmployeeStatus.Active, OutOfOfficeBalance = 10 },
                    new Employee { FullName = "John Doe", Email = "john.doe@example.com", UserName = "john.doe@example.com", SubdivisionId = 2, PositionId = 1, Status = EmployeeStatus.Active, OutOfOfficeBalance = 15 },
                    new Employee { FullName = "Jane Smith", Email = "jane.smith@example.com", UserName = "jane.smith@example.com", SubdivisionId = 1, PositionId = 4, Status = EmployeeStatus.Active, OutOfOfficeBalance = 12 },
                    new Employee { FullName = "Mike Johnson", Email = "mike.johnson@example.com", UserName = "mike.johnson@example.com", SubdivisionId = 3, PositionId = 5, Status = EmployeeStatus.Active, OutOfOfficeBalance = 8 },
                    new Employee { FullName = "Emily Davis", Email = "emily.davis@example.com", UserName = "emily.davis@example.com", SubdivisionId = 2, PositionId = 5, Status = EmployeeStatus.Active, OutOfOfficeBalance = 20 },
                    new Employee { FullName = "Daniel Brown", Email = "daniel.brown@example.com", UserName = "daniel.brown@example.com", SubdivisionId = 1, PositionId = 6, Status = EmployeeStatus.Active, OutOfOfficeBalance = 25 },
                    new Employee { FullName = "Sophia Wilson", Email = "sophia.wilson@example.com", UserName = "sophia.wilson@example.com", SubdivisionId = 4, PositionId = 7, Status = EmployeeStatus.Active, OutOfOfficeBalance = 18 },
                    new Employee { FullName = "Chris Martin", Email = "chris.martin@example.com", UserName = "chris.martin@example.com", SubdivisionId = 2, PositionId = 8, Status = EmployeeStatus.Active, OutOfOfficeBalance = 30 },
                    new Employee { FullName = "Anna Taylor", Email = "anna.taylor@example.com", UserName = "anna.taylor@example.com", SubdivisionId = 1, PositionId = 9, Status = EmployeeStatus.Active, OutOfOfficeBalance = 22 },
                    new Employee { FullName = "David Anderson", Email = "david.anderson@example.com", UserName = "david.anderson@example.com", SubdivisionId = 3, PositionId = 10, Status = EmployeeStatus.Active, OutOfOfficeBalance = 28 }
                };

                for (int i = 5, j = 0; i < employees.Length && j < imageFilePaths.Length; i++, j++)
                {
                    byte[] imageBytes = await File.ReadAllBytesAsync(imageFilePaths[j]);
                    var base64 = Convert.ToBase64String(imageBytes);
                    employees[i].Photo = new Photo { Base64Data = base64 };
                }

                foreach (var employee in employees)
                {
                    await userManager.CreateAsync(employee, "1234qwer");
                    await userManager.AddToRoleAsync(employee, "Employee");
                }

                var adminEmployee = employees.FirstOrDefault();
                await userManager.AddToRoleAsync(adminEmployee, "Admin");

                var hrManagerEmployee = employees[2];
                hrManagerEmployee.EmployeePartner = adminEmployee;
                await userManager.AddToRoleAsync(hrManagerEmployee, "HRManager");

                for (int i = 3; i < employees.Length; i++)
                {
                    employees[i].EmployeePartner = hrManagerEmployee;
                }

                var projectManager1 = employees[4];
                await userManager.AddToRoleAsync(projectManager1, "ProjectManager");
                var projectManager2 = employees[3];
                await userManager.AddToRoleAsync(projectManager2, "ProjectManager");

                await SeedLeaveRequestsAsync(context);
                await SeedApprovalRequestsAsync(context);
                await SeedProjectsAsync(context);

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedSubdivisionsAsync(OutOfOfficeDbContext context)
        {
            if (!await context.Subdivisions.AnyAsync())
            {
                var subdivisions = new[]
                {
                    new Subdivision { Name = "HR Department" },
                    new Subdivision { Name = "IT Department" },
                    new Subdivision { Name = "Marketing Department" },
                    new Subdivision { Name = "Sales Department" }
                };

                await context.Subdivisions.AddRangeAsync(subdivisions);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAbsenceReasonsAsync(OutOfOfficeDbContext context)
        {
            if (!await context.AbsenceReasons.AnyAsync())
            {
                var absenceReasons = new[]
                {
                    new AbsenceReason { ReasonTitle = "Sick Leave" },
                    new AbsenceReason { ReasonTitle = "Vacation" },
                    new AbsenceReason { ReasonTitle = "Personal Leave" },
                    new AbsenceReason { ReasonTitle = "Emergency" },
                    new AbsenceReason { ReasonTitle = "Training" },
                    new AbsenceReason { ReasonTitle = "Conference" }
                };

                await context.AbsenceReasons.AddRangeAsync(absenceReasons);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedLeaveRequestsAsync(OutOfOfficeDbContext context)
        {

            var leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest { EmployeeId = 2, StartDate = new DateTime(2024, 8, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 8, 5, 0, 0, 0, DateTimeKind.Local), Comment = "Vacation", Status = RequestStatus.New, AbsenceReasonId = 2 },
                new LeaveRequest { EmployeeId = 2, StartDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Local), Comment = "Medical", Status = RequestStatus.New, AbsenceReasonId = 1 },
                new LeaveRequest { EmployeeId = 3, StartDate = new DateTime(2024, 7, 15, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 7, 20, 0, 0, 0, DateTimeKind.Local), Comment = "Sick leave", Status = RequestStatus.Approved, AbsenceReasonId = 1 },
                new LeaveRequest { EmployeeId = 4, StartDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 9, 10, 0, 0, 0, DateTimeKind.Local), Comment = "Family emergency", Status = RequestStatus.Pending, AbsenceReasonId = 3 },
                new LeaveRequest { EmployeeId = 5, StartDate = new DateTime(2024, 10, 5, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Local), Comment = "Holiday", Status = RequestStatus.New, AbsenceReasonId = 2 },
                new LeaveRequest { EmployeeId = 6, StartDate = new DateTime(2024, 11, 5, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 11, 12, 0, 0, 0, DateTimeKind.Local), Comment = "Personal reasons", Status = RequestStatus.Rejected, AbsenceReasonId = 3 },
                new LeaveRequest { EmployeeId = 7, StartDate = new DateTime(2024, 12, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 7, 0, 0, 0, DateTimeKind.Local), Comment = "Training", Status = RequestStatus.New, AbsenceReasonId = 5 },
                new LeaveRequest { EmployeeId = 8, StartDate = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 20, 0, 0, 0, DateTimeKind.Local), Comment = "Conference", Status = RequestStatus.Approved, AbsenceReasonId = 6 },
                new LeaveRequest { EmployeeId = 9, StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Local), Comment = "Medical", Status = RequestStatus.New, AbsenceReasonId = 1 },
                new LeaveRequest { EmployeeId = 10, StartDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Local), Comment = "Workshop", Status = RequestStatus.Pending, AbsenceReasonId = 5 }
            };

            await context.LeaveRequests.AddRangeAsync(leaveRequests);
            await context.SaveChangesAsync();
        }

        private static async Task SeedApprovalRequestsAsync(OutOfOfficeDbContext context)
        {
            var hrManager = await context.Employees.FirstOrDefaultAsync(e => e.UserName == "jane.smith@example.com") ?? throw new KeyNotFoundException("HR Manager jane.smith@example.com not found (seeding approval request is not possible without HR)");

            var approvalRequests = new List<ApprovalRequest>
            {
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 1, Status = RequestStatus.Approved, Comment = "Approved by HR" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 2, Status = RequestStatus.New, Comment = "Pending approval" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 3, Status = RequestStatus.Approved, Comment = "Approved by HR" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 4, Status = RequestStatus.Pending, Comment = "Pending review" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 5, Status = RequestStatus.New, Comment = "Pending approval" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 6, Status = RequestStatus.Rejected, Comment = "Rejected by HR" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 7, Status = RequestStatus.New, Comment = "Pending approval" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 8, Status = RequestStatus.Approved, Comment = "Approved by HR" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 9, Status = RequestStatus.New, Comment = "Pending approval" },
                new ApprovalRequest { ApproverId = hrManager.Id, LeaveRequestId = 10, Status = RequestStatus.Pending, Comment = "Pending review" }
            };

            await context.ApprovalRequests.AddRangeAsync(approvalRequests);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProjectsAsync(OutOfOfficeDbContext context)
        {
            var projectManager1 = await context.Employees.FirstOrDefaultAsync(e => e.UserName == "emily.davis@example.com");
            var projectManager2 = await context.Employees.FirstOrDefaultAsync(e => e.UserName == "mike.johnson@example.com");

            if (projectManager1 == null)
            {
                throw new KeyNotFoundException("Project manager not found.");
            }

            var projects = new List<Project>
            {
                new Project { ProjectType = ProjectType.Development, StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 6, 30, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager1.Id, Comment = "New software development", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.MarketingCampaign, StartDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 8, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager2.Id, Comment = "Marketing campaign for Q2", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.Research, StartDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager1.Id, Comment = "Research for new product", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.ProductLaunch, StartDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 11, 30, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager2.Id, Comment = "Launch of new product line", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.Development, StartDate = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager1.Id, Comment = "Backend development for new app", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.MarketingCampaign, StartDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 10, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager2.Id, Comment = "Summer marketing campaign", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.Research, StartDate = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager1.Id, Comment = "User experience research", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.ProductLaunch, StartDate = new DateTime(2024, 8, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager2.Id, Comment = "Product launch preparation", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.Development, StartDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager1.Id, Comment = "Front-end development for new website", Status = ProjectStatus.Ongoing },
                new Project { ProjectType = ProjectType.MarketingCampaign, StartDate = new DateTime(2024, 10, 1, 0, 0, 0, DateTimeKind.Local), EndDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Local), ProjectManagerId = projectManager2.Id, Comment = "Holiday marketing campaign", Status = ProjectStatus.Ongoing }
            };

            await context.Projects.AddRangeAsync(projects);
            await context.SaveChangesAsync();
        }

        private static string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
