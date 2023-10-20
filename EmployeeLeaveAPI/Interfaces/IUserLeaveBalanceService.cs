using EmployeeLeaveAPI.DTOs;

namespace EmployeeLeaveAPI.Interfaces;

public interface IUserLeaveBalanceService
{
    Task<List<UserLeaveBalanceDTO>> GetUserDaysLeftAsync(int userId);
    Task<bool> HasEnoughDaysLeftAsync(int userId, int leaveTypeId, DateTime startDate, DateTime endDate);
}