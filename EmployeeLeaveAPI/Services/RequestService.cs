using EmployeeLeaveAPI.Interfaces;

namespace EmployeeLeaveAPI.Services;

public class RequestService : IRequestService
{
    public async Task<(bool isOk, string message)> CheckValidDates(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return (false, "Start date must be before end date");
        }
        
        if (startDate < DateTime.Now)
        {
            return (false, "Start date must be in the future");
        }

        return (true, "Dates are valid");
    }
}