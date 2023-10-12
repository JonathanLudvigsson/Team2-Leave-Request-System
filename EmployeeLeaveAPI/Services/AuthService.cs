using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeLeaveAPI.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(ILogger<AuthService> logger, IUserRepository userRepository, IConfiguration configuration)
    {
        _logger = logger;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResult> LoginUser(LoginDTO model)
    {
        try
        {
            _logger.LogInformation("Logging in user with email {Email}", model.Email);

            var user = await _userRepository.GetByEmail(model.Email);

            if (user == null)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "Invalid password"
                };
            }

            var token = GenerateJwt(user);

            return new LoginResult
            {
                Token = token,
                IsSuccess = true,
                Message = "Login successful"
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error logging in user");

            return new LoginResult
            {
                IsSuccess = false,
                Message = "Error logging in user: " + e.Message
            };
        }
    }

    public string GeneratePasswordHash(string password)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating password hash");
            throw new Exception("Error generating password hash: " + e.Message);
        }
    }

    private string GenerateJwt(User user)
    {
        try
        {
            var key = _configuration["Jwt:Key"] ?? throw new Exception("JWT key not found");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("UserId", user.ID.ToString()),
                new("Email", user.Email),
                new("FirstName", user.Name),
                new("IsAdmin", user.IsAdmin.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error generating JWT token");
            throw new Exception("Error generating JWT token: " + e.Message);
        }
    }
}