using System.ComponentModel.DataAnnotations;


namespace EmployeeLeaveAPI.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public Status LeaveStatus { get; set; } = Status.Pending;
        public User FKUser { get; set; }
        public int UserID { get; set; }
        public LeaveType FKLeaveType { get; set; }
        public int LeaveTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
