using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAPI.Models
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeID { get; set; }
        public string Name { get; set; }
        public int MaximumDays { get; set; }
    }
}
