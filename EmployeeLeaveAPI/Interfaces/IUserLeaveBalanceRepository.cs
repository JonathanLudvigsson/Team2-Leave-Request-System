namespace EmployeeLeaveAPI.Interfaces;

public interface IUserLeaveBalanceRepository
{
    Task<int> GetUserLeaveUsedDaysByLeaveType(int userId, int leaveTypeId);
}