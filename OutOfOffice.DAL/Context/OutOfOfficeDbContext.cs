﻿using Microsoft.EntityFrameworkCore;
using OutOfOffice.DAL.Models;


namespace OutOfOffice.DAL.Context
{
    public class OutOfOfficeDbContext : DbContext
    {
        public OutOfOfficeDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public OutOfOfficeDbContext()
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

            modelBuilder.Entity<ApprovalRequest>()
                .HasOne(ar => ar.LeaveRequest)
                .WithMany(lr => lr.ApprovalRequests)
                .HasForeignKey(ar => ar.LeaveRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalRequest>()
                .HasOne(ar => ar.Approver)
                .WithMany(e => e.ApprovalRequests)
                .HasForeignKey(ar => ar.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}