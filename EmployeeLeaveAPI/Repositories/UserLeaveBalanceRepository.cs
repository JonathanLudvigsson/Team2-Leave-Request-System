using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Repositories;

public class UserLeaveBalanceRepository : IUserLeaveBalanceRepository
{
    private readonly AppDbContext _context;

    public UserLeaveBalanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserLeaveBalance?>> GetByUserId(int userId)
    {
        return await _context.UserLeaveBalances.Where(ulb => ulb.UserID == userId).ToListAsync();
    }

    public async Task<IEnumerable<UserLeaveBalance?>>? AddBalancesForNewUser(int userId, IEnumerable<LeaveType> leaveTypes)
    {
        List<UserLeaveBalance> balances = new();

        foreach (var leaveType in leaveTypes)
        {
            balances.Add(new UserLeaveBalance
            {
                UserID = userId,
                LeaveTypeID = leaveType.LeaveTypeID,
                MaximumDays = leaveType.MaximumDays,
                DaysUsed = 0
            });
        }

        await _context.UserLeaveBalances.AddRangeAsync(balances);
        await _context.SaveChangesAsync();
        return balances;
    }
}