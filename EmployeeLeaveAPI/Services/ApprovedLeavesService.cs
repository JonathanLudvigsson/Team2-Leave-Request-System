using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Services;

public class ApprovedLeavesService : IApprovedLeavesService
{
    private readonly IRepository<ApprovedLeave> _approvedLeavesRepository;
    private readonly ILogger<ApprovedLeavesService> _logger;

    public ApprovedLeavesService(IRepository<ApprovedLeave> approvedLeavesRepository,
        ILogger<ApprovedLeavesService> logger)
    {
        _approvedLeavesRepository = approvedLeavesRepository;
        _logger = logger;
    }

    public async Task CreateApprovedLeave(DateTime startDate, DateTime endDate, int userId, int leaveTypeId,
        int requestId)
    {
        try
        {
            int totalDays = await CalculateActualLeaveDays(startDate, endDate);
            var approvedLeave = new ApprovedLeave
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId,
                LeaveTypeId = leaveTypeId,
                RequestId = requestId,
                TotalDays = totalDays,
                ApprovedDate = DateTime.Now
            };
            await _approvedLeavesRepository.Create(approvedLeave);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception("Error creating approved leave: " + e.Message);
        }
    }
    
    public async Task<int> CalculateActualLeaveDays(DateTime startDate, DateTime endDate)
    {
        var totalDays = (endDate - startDate).Days + 1;
        for (var i = 0; i < totalDays; i++)
        {
            var date = startDate.AddDays(i);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                totalDays--;
            }
        }

        return totalDays;
    }
}