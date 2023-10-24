using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces;

public interface IEmailService
{
    Task<(bool isSuccess, string message, Email? email)> CreateEmail(int userId, Status newStatus);
    Task<(bool isSuccess, string message)> SaveEmailToDbAsync(Email email);
    Task<(bool isSuccess, string message)> EnqueueEmail(Email email);
    Task<(bool isSuccess, string message)> SendEmail(Email email);
    
}