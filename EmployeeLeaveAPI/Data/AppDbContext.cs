using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<ApprovedLeave> ApprovedLeaves { get; set; }
        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApprovedLeave>()
                .HasIndex(p => p.RequestId)
                .IsUnique();

            modelBuilder.Entity<ApprovedLeave>()
                .HasOne(p => p.User)
                .WithMany() // Or with navigation property if it exists.
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovedLeave>()
                .HasOne(p => p.LeaveType)
                .WithMany() // Or with navigation property if it exists.
                .HasForeignKey(p => p.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveType>().HasData(
                new LeaveType
                {
                    LeaveTypeID = 1,
                    Name = "Vacation",
                    MaximumDays = 20
                },
                new LeaveType
                {
                    LeaveTypeID = 2,
                    Name = "Sick Leave",
                    MaximumDays = 10
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = 1,
                    Name = "John Doe",
                    Address = "123 Main St",
                    Email = "john@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    IsAdmin = false
                },
                new User
                {
                    ID = 2,
                    Name = "Admin User",
                    Address = "456 Admin Ave",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    IsAdmin = true
                }
            );
        }
    }
}