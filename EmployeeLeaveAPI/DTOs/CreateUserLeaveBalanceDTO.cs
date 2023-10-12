namespace EmployeeLeaveAPI.DTOs;

public class CreateUserLeaveBalanceDTO
{
    public int LeaveTypeID { get; set; }
    public int UserID { get; set; }
    public int MaximumDays { get; set; }
    public int DaysUsed { get; set; }
}