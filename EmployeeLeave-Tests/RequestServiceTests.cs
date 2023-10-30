using EmployeeLeaveAPI.Services;

namespace EmployeeLeave_Tests;

public class RequestServiceTests
{
    private readonly RequestService _requestService = new();

    [Fact]
    public async Task CheckValidDates_StartDateAfterEndDate_ReturnsInvalid()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(2);
        var endDate = DateTime.Now.AddDays(1);

        // Act
        var (isOk, message) = await _requestService.CheckValidDates(startDate, endDate);

        // Assert
        Assert.False(isOk);
        Assert.Equal("Start date must be before end date", message);
    }

    [Fact]
    public async Task CheckValidDates_StartDateInThePast_ReturnsInvalid()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(-2);
        var endDate = DateTime.Now.AddDays(2);

        // Act
        var (isOk, message) = await _requestService.CheckValidDates(startDate, endDate);

        // Assert
        Assert.False(isOk);
        Assert.Equal("Start date must be in the future", message);
    }

    [Fact]
    public async Task CheckValidDates_ValidDates_ReturnsValid()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(2);

        // Act
        var (isOk, message) = await _requestService.CheckValidDates(startDate, endDate);

        // Assert
        Assert.True(isOk);
        Assert.Equal("Dates are valid", message);
    }
}