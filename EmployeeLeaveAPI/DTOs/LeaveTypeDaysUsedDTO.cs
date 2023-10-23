namespace EmployeeLeaveAPI.DTOs
{
    public class LeaveTypeDaysUsedDTO
    {
        public int LeaveTypeID { get; set; }
        public string Name { get; set; }
        public int MaximumDays { get; set; }
        public int TotalDaysUsed { get; set; }
    }
}
