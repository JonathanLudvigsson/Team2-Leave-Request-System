using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _context;

        public RequestRepository(AppDbContext dbInject)
        {
            _context = dbInject;
        }
        public async Task<IEnumerable<Request?>>? GetRequestsFromUser(int userID)
        {
            return await _context.Requests.Where(r => r.UserID == userID).ToListAsync();
        }
    }
}
