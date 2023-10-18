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
            var approvedLeave = new ApprovedLeave
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId,
                LeaveTypeId = leaveTypeId,
                RequestId = requestId,
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
}