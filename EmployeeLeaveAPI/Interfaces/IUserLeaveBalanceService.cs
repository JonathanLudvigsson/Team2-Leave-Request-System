namespace EmployeeLeaveAPI.Interfaces;

public interface IUserLeaveBalanceService
{
    Task<int> GetUserDaysLeftByLeavetype(int userId, int leaveTypeId);
}