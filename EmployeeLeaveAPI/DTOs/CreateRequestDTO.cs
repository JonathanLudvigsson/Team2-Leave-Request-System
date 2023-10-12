using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.DTOs
{
    public class CreateRequestDTO
    {
        public Status LeaveStatus { get; set; } = Status.Pending;
        public int UserID { get; set; }
        public int LeaveTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
