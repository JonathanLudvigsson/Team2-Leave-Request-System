namespace EmployeeLeaveAPI.Interfaces;

public interface IRequestService
{
    Task<(bool isOk, string message)> CheckValidDates(DateTime startDate, DateTime endDate);
}