using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeLeaveAPI.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        public Status LeaveStatus { get; set; } = Status.Pending;
        [JsonIgnore]
        public User FKUser { get; set; }
        public int UserID { get; set; }
        [JsonIgnore]
        public LeaveType? FKLeaveType { get; set; }
        public int LeaveTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
