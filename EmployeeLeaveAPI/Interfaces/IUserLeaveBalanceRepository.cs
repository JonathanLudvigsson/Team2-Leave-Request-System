using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces;

public interface IUserLeaveBalanceRepository
{
    Task<IEnumerable<UserLeaveBalance?>>? GetByUserId(int userId);
    Task<IEnumerable<UserLeaveBalance?>>? AddBalancesForNewLeaveType(LeaveType newLeave);
}