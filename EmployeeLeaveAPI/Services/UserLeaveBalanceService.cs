using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Services;

public class UserLeaveBalanceService : IUserLeaveBalanceService
{
    private readonly IApprovedLeavesRepository _approvedLeaveRepository;
    private readonly IRepository<LeaveType> _leaveTypeRepository;
    private readonly ILogger<UserLeaveBalanceService> _logger;
    
    public UserLeaveBalanceService(IApprovedLeavesRepository approvedLeaveRepository, IRepository<LeaveType> leaveTypeRepository, ILogger<UserLeaveBalanceService> logger)
    {
        _approvedLeaveRepository = approvedLeaveRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
    }

    public async Task<List<UserLeaveBalanceDTO>> GetUserDaysLeftAsync(int userId)
    {
        try
        {
            var approvedLeaves = await _approvedLeaveRepository.GetByUserId(userId);

            var leaveGroups = approvedLeaves
                .GroupBy(x=> x.LeaveTypeId)
                .Select(g => new
                {
                    LeaveTypeId = g.Key,
                    DaysTaken = g.Sum(x => (x.TotalDays))
                }).ToList();

            var leaveTypes = await _leaveTypeRepository.GetAll();
            
            var result = new List<UserLeaveBalanceDTO>();
            
            foreach (var leaveType in leaveTypes)
            {
                var daysTaken = leaveGroups.FirstOrDefault(x => x.LeaveTypeId == leaveType.LeaveTypeID)?.DaysTaken ?? 0;
                var daysLeft = leaveType.MaximumDays - daysTaken;
                
                result.Add(new UserLeaveBalanceDTO
                {
                    LeaveTypeId = leaveType.LeaveTypeID,
                    DaysLeft = daysLeft,
                    LeaveTypeName = leaveType.Name
                });
            }
            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting leave balance");
            throw new Exception("Error getting leave balance + " + e.Message);
        }
    }
}