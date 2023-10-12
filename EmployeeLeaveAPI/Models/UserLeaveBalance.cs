using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeLeaveAPI.Models
{
    public class UserLeaveBalance
    {
        [Key]
        public int ID { get; set; }
        public int LeaveTypeID { get; set; }
        [JsonIgnore]
        public LeaveType FKLeaveType { get; set; }
        public int UserID { get; set; }
        [JsonIgnore]
        public User FKUser { get; set; }
        public int MaximumDays { get; set; }
        public int DaysUsed { get; set; }
    }
}
