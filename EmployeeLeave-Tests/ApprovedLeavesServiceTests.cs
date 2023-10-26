using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace EmployeeLeave_Tests;

public class ApprovedLeavesServiceTests
{
    private readonly IRepository<ApprovedLeave> _approvedLeavesRepository = Substitute.For<IRepository<ApprovedLeave>>();
    private readonly ILogger<ApprovedLeavesService> _logger = Substitute.For<ILogger<ApprovedLeavesService>>();

    [Fact]
    public async Task CreateApprovedLeave_CreatesLeave_WithCorrectDays()
    {
        // Arrange
        var service = new ApprovedLeavesService(_approvedLeavesRepository, _logger);
        var startDate = new DateTime(2023, 10, 1); // Assuming this is a Sunday
        var endDate = new DateTime(2023, 10, 5);

        // Act
        await service.CreateApprovedLeave(startDate, endDate, 1, 1, 123);

        // Assert
        await _approvedLeavesRepository.Received(1).Create(Arg.Is<ApprovedLeave>(x =>
                x.StartDate == startDate &&
                x.EndDate == endDate &&
                x.UserId == 1 &&
                x.LeaveTypeId == 1 &&
                x.RequestId == 123 &&
                x.TotalDays == 4 // Because Oct 1 is Sunday
        ));
    }

    [Fact]
    public async Task CalculateActualLeaveDays_ExcludesWeekends()
    {
        // Arrange
        var service = new ApprovedLeavesService(_approvedLeavesRepository, _logger);
        var startDate = new DateTime(2023, 10, 1); // Assuming this is a Sunday
        var endDate = new DateTime(2023, 10, 5);

        // Act
        var totalDays = await service.CalculateActualLeaveDays(startDate, endDate);

        // Assertt
        Assert.Equal(4, totalDays); // Because Oct 1 is Sunday
    }
}