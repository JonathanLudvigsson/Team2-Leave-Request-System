using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Repositories;

public class ApprovedLeavesRepository : IApprovedLeavesRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ApprovedLeavesRepository> _logger;

    public ApprovedLeavesRepository(AppDbContext context, ILogger<ApprovedLeavesRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<ApprovedLeave>> GetByLeaveType(int leaveTypeId)
    {
        try
        {
            return await _context.ApprovedLeaves.Where(x => x.LeaveTypeId == leaveTypeId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting approved leaves by leave type");
            return null;
        }
    }

    public async Task<IEnumerable<ApprovedLeave>> GetByLeaveTypeAndDateRange(int leaveTypeId, DateTime startDate,
        DateTime endDate)
    {
        try
        {
            return await _context.ApprovedLeaves
                .Where(x => x.LeaveTypeId == leaveTypeId && x.StartDate >= startDate && x.EndDate <= endDate)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting approved leaves by leave type and date range");
            return null;
        }
    }
    
    public async Task<IEnumerable<ApprovedLeave>> GetByUserId(int userId)
    {
        try
        {
            return await _context.ApprovedLeaves.Where(x => x.UserId == userId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting approved leaves by user id");
            return null;
        }
    }
}