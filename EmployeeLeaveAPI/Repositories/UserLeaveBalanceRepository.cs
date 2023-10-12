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

    public async Task<IEnumerable<UserLeaveBalance?>> AddBalancesForNewLeaveType(LeaveType newLeave)
    {
        List<UserLeaveBalance> addedBalances = new List<UserLeaveBalance>();
        foreach(User user in _context.Users.Where(u => !u.IsAdmin))
        {

            UserLeaveBalance newLeaveBalance = new UserLeaveBalance
            {
                LeaveTypeID = newLeave.LeaveTypeID,
                FKLeaveType = newLeave,
                UserID = user.ID,
                FKUser = user,
                MaximumDays = newLeave.MaximumDays,
                DaysUsed = 0
            };

            await _context.UserLeaveBalances.AddAsync(newLeaveBalance);
            addedBalances.Add(newLeaveBalance);
        }

        await _context.SaveChangesAsync();
        return addedBalances;
    }
}