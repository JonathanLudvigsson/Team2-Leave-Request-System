using NSubstitute;
using EmployeeLeaveAPI.Services;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeLeave_Tests
{
    public class UserLeaveBalanceServiceTests
    {
        private readonly IApprovedLeavesRepository _mockApprovedLeaveRepo;
        private readonly IRepository<LeaveType> _mockLeaveTypeRepo;
        private readonly ILogger<UserLeaveBalanceService> _mockLogger;
        private readonly UserLeaveBalanceService _service;

        public UserLeaveBalanceServiceTests()
        {
            _mockApprovedLeaveRepo = Substitute.For<IApprovedLeavesRepository>();
            _mockLeaveTypeRepo = Substitute.For<IRepository<LeaveType>>();
            _mockLogger = Substitute.For<ILogger<UserLeaveBalanceService>>();
            
            _service = new UserLeaveBalanceService(_mockApprovedLeaveRepo, _mockLeaveTypeRepo, _mockLogger);
        }

        [Fact]
        public async Task GetUserDaysLeftAsync_ReturnsCorrectDaysLeft()
        {
            // Arrange
            var userId = 1;

            _mockApprovedLeaveRepo.GetByUserId(userId)
                .Returns(new List<ApprovedLeave>
                {
                    new ApprovedLeave { LeaveTypeId = 1, TotalDays = 5 },
                    new ApprovedLeave { LeaveTypeId = 1, TotalDays = 3 }
                });

            _mockLeaveTypeRepo.GetAll()
                .Returns(new List<LeaveType>
                {
                    new LeaveType { LeaveTypeID = 1, MaximumDays = 30, Name = "Vacation" }
                });

            // Act
            var result = await _service.GetUserDaysLeftAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(22, result[0].DaysLeft); // 30 max days - 5 - 3
        }

        [Fact]
        public async Task HasEnoughDaysLeftAsync_ReturnsTrueIfDaysLeft()
        {
            // Arrange
            var userId = 1;
            var leaveTypeId = 1;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 10);

            _mockApprovedLeaveRepo.GetByUserId(userId)
                .Returns(new List<ApprovedLeave>
                {
                    new ApprovedLeave { LeaveTypeId = 1, TotalDays = 5 }
                });

            _mockLeaveTypeRepo.Get(leaveTypeId)
                .Returns(new LeaveType { LeaveTypeID = 1, MaximumDays = 30, Name = "Vacation" });

            // Act
            var result = await _service.HasEnoughDaysLeftAsync(userId, leaveTypeId, startDate, endDate);

            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public async Task HasNotEnoughDaysLeftAsync_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var leaveTypeId = 1;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 31);

            _mockApprovedLeaveRepo.GetByUserId(userId)
                .Returns(new List<ApprovedLeave>
                {
                    new ApprovedLeave { LeaveTypeId = 1, TotalDays = 5 }
                });

            _mockLeaveTypeRepo.Get(leaveTypeId)
                .Returns(new LeaveType { LeaveTypeID = 1, MaximumDays = 30, Name = "Vacation" });

            // Act
            var result = await _service.HasEnoughDaysLeftAsync(userId, leaveTypeId, startDate, endDate);

            // Assert
            Assert.True(result);
        }
    }
}