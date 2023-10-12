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
        public DbSet<UserLeaveBalance> UserLeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserLeaveBalance>()
                .HasIndex(ul => new { ul.UserID, ul.LeaveTypeID })
                .IsUnique();

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

            modelBuilder.Entity<UserLeaveBalance>().HasData(
                new UserLeaveBalance
                {
                    ID = 1,
                    UserID = 1,
                    LeaveTypeID = 1,
                    MaximumDays = 20,
                    DaysUsed = 5
                },
                new UserLeaveBalance
                {
                    ID = 2,
                    UserID = 1,
                    LeaveTypeID = 2,
                    MaximumDays = 10,
                    DaysUsed = 3
                }
            );
        }
    }
}