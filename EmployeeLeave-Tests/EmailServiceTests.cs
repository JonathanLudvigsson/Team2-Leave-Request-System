using Castle.Core.Configuration;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace EmployeeLeave_Tests;

public class EmailServiceTests
{
    private readonly IRepository<User> _mockUserRepository;
    private readonly ILogger<EmailService> _mockLogger;
    private readonly IRepository<Email> _mockEmailRepository;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _mockConfiguration;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _mockUserRepository = Substitute.For<IRepository<User>>();
        _mockLogger = Substitute.For<ILogger<EmailService>>();
        _mockEmailRepository = Substitute.For<IRepository<Email>>();
        _mockConfiguration = Substitute.For<IConfiguration>();
        _emailService = new EmailService(_mockUserRepository, _mockLogger, _mockEmailRepository, _mockConfiguration);
    }

    [Fact]
    public async Task CreateEmail_ValidUserId_ReturnsEmail()
    {
        // Arrange
        int userId = 1;
        var user = new User { Email = "test@test.com" };
        _mockUserRepository.Get(userId).Returns(user);

        // Act
        var (isSuccess, message, email) = await _emailService.CreateEmail(userId, Status.Approved);

        // Assert
        Assert.True(isSuccess);
        Assert.Equal("Email created", message);
        Assert.NotNull(email);
    }

    [Fact]
    public async Task SaveEmailToDbAsync_ValidEmail_ReturnsSuccess()
    {
        // Arrange
        var email = new Email { To = "test@test.com", Subject = "Test", Body = "Test body" };

        // Act
        var (isSuccess, message) = await _emailService.SaveEmailToDbAsync(email);

        // Assert
        Assert.True(isSuccess);
        Assert.Equal("Email saved to db", message);
    }
}