using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAPI.Models
{
    public class UserLeaveBalance
    {
        [Key]
        public int ID { get; set; }
        public int LeaveTypeID { get; set; }
        public LeaveType FKLeaveType { get; set; }
        public int UserID { get; set; }
        public User FKUser { get; set; }
        public int MaximumDays { get; set; }
        public int DaysUsed { get; set; }
    }
}
