namespace EmployeeLeaveAPI.DTOs;

public class CreateUserDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Password { get; set; }
    public bool IsAdmin { get; set; }
}