using AutoMapper;
using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequest;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services;

public class LeaveRequestService : BaseHttpService, ILeaveRequestService
{
    private readonly IMapper _mapper;

    public LeaveRequestService(IClient client,
        ILocalStorageService localStorage,
        IMapper mapper) : base(client, localStorage)
    {
        _mapper = mapper;

    }
    public async Task<Response<Guid>> ApproveLeaveRequest(int id, bool approved)
    {
        try
        {
            var response = new Response<Guid>();
            var request = new ChangeLeaveRequestApprovalCommand { Approved = approved, LeaveRequestId = id };
            await _client.UpdateApprovalAsync(request);
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }

    }

    public Task<Response<Guid>> CancelLeaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<Guid>> CreateLeaveRequest(LeaveRequestVm leaveRequest)
    {
        try
        {
            var response = new Response<Guid>();
            CreateLeaveRequestCommand createLeaveRequest =
                _mapper.Map<CreateLeaveRequestCommand>(leaveRequest);

            await _client.LeaveRequestsPOSTAsync(createLeaveRequest);
            return response;
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }

    public Task DeleteLeaveRequest(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<AdminLeaveRequestViewVm> GetAdminLeaveRequestList()
    {
        var leaveRequests = await _client.LeaveRequestsAllAsync(isLoggedInUser: false);

        var model = new AdminLeaveRequestViewVm
        {
            TotalRequests = leaveRequests.Count,
            ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
            PendingRequests = leaveRequests.Count(q => q.Approved == null),
            RejectedRequests = leaveRequests.Count(q => q.Approved == false),
            LeaveRequests = _mapper.Map<List<LeaveRequestVm>>(leaveRequests)
        };
        return model;

    }

    public async Task<LeaveRequestVm> GetLeaveRequest(int id)
    {
        var leaveRequest = await _client.LeaveRequestsGETAsync(id);
        return _mapper.Map<LeaveRequestVm>(leaveRequest);
    }

    public Task<EmployeeLeaveRequestViewVm> GetUserLeaveRequests()
    {
        throw new NotImplementedException();
    }
}