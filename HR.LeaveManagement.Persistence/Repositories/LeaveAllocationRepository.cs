using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
    {
    }

    private IQueryable<LeaveAllocation> leaveAllocations()
    {
        var leaveAllocations = _context.LeaveAllocations.Include(i => i.LeaveType);
        return leaveAllocations;
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        return await leaveAllocations().SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        return await leaveAllocations().ToListAsync();
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        return await leaveAllocations()
            .Where(p => p.EmployeeId == userId)
            .ToListAsync();
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    {
        return await _context.LeaveAllocations.AnyAsync(p =>
            p.EmployeeId == userId && p.LeaveTypeId == leaveTypeId && p.Period == period);
    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await _context.AddRangeAsync(allocations);
        await _context.SaveChangesAsync();
    }
     
    public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
    {
        return await leaveAllocations()
            .FirstOrDefaultAsync(p => p.EmployeeId == userId 
                        && p.LeaveTypeId == leaveTypeId);
    }
}