using System;

namespace HR.LeaveManagement.BlazorUI.Models.LeaveRequest;

public class AdminLeaveRequestViewVm
{
    public int TotalRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int PendingRequests { get; set; }
    public int RejectedRequests { get; set; }
    public List<LeaveRequestVm> LeaveRequests { get; set; } = new List<LeaveRequestVm>();

}
