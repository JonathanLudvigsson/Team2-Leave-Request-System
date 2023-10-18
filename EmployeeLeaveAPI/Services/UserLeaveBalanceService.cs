using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Services;

public class UserLeaveBalanceService : IUserLeaveBalanceService
{
    private readonly IUserLeaveBalanceRepository _userLeaveBalanceRepository;
    private readonly IRepository<LeaveType> _leaveTypeRepository;
    private readonly ILogger<UserLeaveBalanceService> _logger;

    public UserLeaveBalanceService(IUserLeaveBalanceRepository userLeaveBalanceRepository,
        IRepository<LeaveType> leaveTypeRepository, ILogger<UserLeaveBalanceService> logger)
    {
        _userLeaveBalanceRepository = userLeaveBalanceRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _logger = logger;
    }

    public async Task<int> GetUserDaysLeftByLeavetype(int userId, int leaveTypeId)
    {
        try
        {
            var leaveType = await _leaveTypeRepository.Get(leaveTypeId);
            var userLeaveUsedDays =
                await _userLeaveBalanceRepository.GetUserLeaveUsedDaysByLeaveType(userId, leaveTypeId);
            return leaveType.MaximumDays - userLeaveUsedDays;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting leave balance");
            throw new Exception("Error getting leave balance + " + e.Message);
        }
    }
}