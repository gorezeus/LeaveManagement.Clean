using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestRepository(HrDatabaseContext context) : base(context)
    {
    }

    private IQueryable<LeaveRequest> LeaveRequests()
    {
        var leaveRequest = _context.LeaveRequests.Include(i => i.LeaveType).AsNoTracking();
        return leaveRequest;
    }

    public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
    {
        var leaveRequest = await LeaveRequests()
            .SingleOrDefaultAsync(p => p.Id == id);
        return leaveRequest;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
    {
        var leaveRequest = await LeaveRequests()
            .ToListAsync();
        return leaveRequest;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
    {
        var leaveRequest = await LeaveRequests()
            .Where(p => p.RequestingEmployeeId == userId)
            .ToListAsync();
        return leaveRequest;
    }
}