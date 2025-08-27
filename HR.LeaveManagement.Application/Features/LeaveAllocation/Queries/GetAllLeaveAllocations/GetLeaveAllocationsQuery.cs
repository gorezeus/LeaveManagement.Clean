using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllLeaveAllocations
{
    public record GetLeaveAllocationsQuery : IRequest<List<LeaveAllocationDto>>
    {
    }
}
