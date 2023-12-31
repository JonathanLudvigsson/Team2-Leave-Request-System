using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces;

public interface IApprovedLeavesService
{
    Task CreateApprovedLeave(DateTime startDate, DateTime endDate, int userId, int leaveTypeId, int requestId);
    Task<int> CalculateActualLeaveDays(DateTime startDate, DateTime endDate);
}