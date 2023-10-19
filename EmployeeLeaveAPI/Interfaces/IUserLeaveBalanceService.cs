using EmployeeLeaveAPI.DTOs;

namespace EmployeeLeaveAPI.Interfaces;

public interface IUserLeaveBalanceService
{
    Task<List<UserLeaveBalanceDTO>> GetUserDaysLeftAsync(int userId);
}