using System.Text.Json.Serialization;

namespace EmployeeLeaveAPI.Models;

public class ApprovedLeave
{
    public int ApprovedLeaveId { get; set; }
    public int RequestId { get; set; }
    [JsonIgnore] public Request Request { get; set; }
    public int LeaveTypeId { get; set; }
    [JsonIgnore] public LeaveType LeaveType { get; set; }
    public int UserId { get; set; }
    [JsonIgnore] public User User { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApprovedDate { get; set; }
}