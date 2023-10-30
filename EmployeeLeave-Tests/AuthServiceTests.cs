using Castle.Core.Configuration;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using EmployeeLeaveAPI.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace EmployeeLeave_Tests;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthService> _mockLogger;
    private readonly IUserRepository _mockUserRepository;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _mockConfiguration;

    public AuthServiceTests()
    {
        _mockLogger = Substitute.For<ILogger<AuthService>>();
        _mockUserRepository = Substitute.For<IUserRepository>();
        _mockConfiguration = Substitute.For<IConfiguration>();

        _authService = new AuthService(_mockLogger, _mockUserRepository, _mockConfiguration);
    }

    [Fact]
    public async Task LoginUser_UserNotFound_ReturnsFailureResult()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "test123" };
        _mockUserRepository.GetByEmail(loginDto.Email).Returns((User)null);

        // Act
        var result = await _authService.LoginUser(loginDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.Message);
    }

    [Fact]
    public async Task LoginUser_InvalidPassword_ReturnsFailureResult()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "wrongpassword" };
        var user = new User { Email = "test@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123") };
        _mockUserRepository.GetByEmail(loginDto.Email).Returns(user);

        // Act
        var result = await _authService.LoginUser(loginDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid password", result.Message);
    }

    [Fact]
    public async Task LoginUser_ValidLogin_ReturnsSuccessResultWithToken()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "test123" };

        var user = new User
        {
            ID = 1,
            Name = "Test User",
            Address = "Test Address",
            Email = "test@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("test123"),
            IsAdmin = false,
            Requests = new List<Request>()
        };

        _mockUserRepository.GetByEmail(loginDto.Email).Returns(user);
        _mockConfiguration["Jwt:Key"].Returns("SomeTestJwtKeyThatIs32CharsLong");
        _mockConfiguration["Jwt:Issuer"].Returns("TestIssuer");
        _mockConfiguration["Jwt:Audience"].Returns("TestAudience");

        // Act
        var result = await _authService.LoginUser(loginDto);

        // Assert
        Assert.Equal("Login successful", result.Message);
        Assert.NotNull(result.Token);
    }
    
    [Fact]
    public void GeneratePasswordHash_ValidPassword_ReturnsHashedPassword()
    {
        // Arrange
        var password = "test123";

        // Act
        var hashedPassword = _authService.GeneratePasswordHash(password);

        // Assert
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
    }

}