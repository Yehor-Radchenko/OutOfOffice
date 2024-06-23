using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.DAL.Context
{
    public class OutOfOfficeDbContext : IdentityDbContext<Employee>
    {
        public OutOfOfficeDbContext(DbContextOptions<OutOfOfficeDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeePartner)
                .WithMany()
                .HasForeignKey(e => e.EmployeePartnerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employee>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ApprovalRequest>()
                .HasOne(ar => ar.LeaveRequest)
                .WithOne(lr => lr.ApprovalRequest)
                .HasForeignKey<ApprovalRequest>(ar => ar.LeaveRequestId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ApprovalRequest>()
               .HasOne(ar => ar.Approver)
               .WithMany(e => e.ApprovalRequests)
               .HasForeignKey(ar => ar.ApproverId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ApprovalRequest>()
               .Property(e => e.Status)
               .HasConversion<string>();

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveRequest>()
               .Property(e => e.Status)
               .HasConversion<string>();

            modelBuilder.Entity<Project>()
               .Property(e => e.Status)
               .HasConversion<string>();
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProjectManager)
                .WithMany(e => e.ManagedProjects)
                .HasForeignKey(p => p.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity(j => j.ToTable("ProjectEmployees"));
        }
    }
}
