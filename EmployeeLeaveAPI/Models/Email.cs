namespace EmployeeLeaveAPI.Models;

public class Email
{
    public int Id { get; set; }
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime? SentDate { get; set; }
    public EmailStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}