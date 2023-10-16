using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request?>>? GetRequestsFromUser(int userID);
    }
}
