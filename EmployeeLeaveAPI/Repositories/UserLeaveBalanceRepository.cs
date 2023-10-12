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
}