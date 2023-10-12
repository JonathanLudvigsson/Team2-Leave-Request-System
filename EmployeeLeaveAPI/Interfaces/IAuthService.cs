using EmployeeLeaveAPI.DTOs;

namespace EmployeeLeaveAPI.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginUser(LoginDTO model);
    string GeneratePasswordHash(string password);
}