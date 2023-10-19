using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces;

public interface IApprovedLeavesRepository
{
    Task<IEnumerable<ApprovedLeave>> GetByLeaveType(int leaveTypeId);
    Task<IEnumerable<ApprovedLeave>> GetByLeaveTypeAndDateRange(int leaveTypeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<ApprovedLeave>> GetByUserId(int userId);
}