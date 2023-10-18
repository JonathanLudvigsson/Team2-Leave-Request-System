using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Repositories;

public class UserLeaveBalanceRepository : IUserLeaveBalanceRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserLeaveBalanceRepository> _logger;

    public UserLeaveBalanceRepository(AppDbContext context, ILogger<UserLeaveBalanceRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> GetUserLeaveUsedDaysByLeaveType(int userId, int leaveTypeId)
    {
        try
        {
            var leaves = await _context.ApprovedLeaves
                .Where(l => l.UserId == userId && l.LeaveTypeId == leaveTypeId)
                .ToListAsync();

            return leaves.Sum(l => (int)(l.EndDate - l.StartDate).TotalDays + 1);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting leave balance");
            throw new Exception("Error getting leave balance + " + e.Message);
        }
    }
}