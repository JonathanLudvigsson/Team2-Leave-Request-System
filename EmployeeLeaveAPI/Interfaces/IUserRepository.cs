using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
}