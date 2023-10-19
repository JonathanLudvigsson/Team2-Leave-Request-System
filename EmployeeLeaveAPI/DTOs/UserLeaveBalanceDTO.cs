namespace EmployeeLeaveAPI.DTOs;

public class UserLeaveBalanceDTO
{
    public int LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; }
    public int DaysLeft { get; set; }
}