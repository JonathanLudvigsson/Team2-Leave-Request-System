namespace EmployeeLeaveAPI.DTOs;

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public string? Token { get; set; }
}