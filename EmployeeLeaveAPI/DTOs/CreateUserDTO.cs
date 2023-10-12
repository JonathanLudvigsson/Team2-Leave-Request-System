namespace EmployeeLeaveAPI.DTOs;

public class CreateUserDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsAdmin { get; set; }
}