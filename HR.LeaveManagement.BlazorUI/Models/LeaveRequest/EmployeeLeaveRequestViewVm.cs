using System;
using HR.LeaveManagement.BlazorUI.Models.LeaveAllocation;

namespace HR.LeaveManagement.BlazorUI.Models.LeaveRequest;

public class EmployeeLeaveRequestViewVm
{
    public List<LeaveAllocationVm> LeaveAllocations { get; set; } = new List<LeaveAllocationVm>();
    public List<LeaveRequestVm> LeaveRequests { get; set; } = new List<LeaveRequestVm>();
}
