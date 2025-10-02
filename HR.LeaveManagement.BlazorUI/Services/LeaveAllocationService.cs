using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services;

public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
{
    public LeaveAllocationService(IClient client,
        ILocalStorageService localStorage) : base(client, localStorage)
    {
    }

    public async Task<Response<Guid>> CreateLeaveAllocations(int leaveTypeId)
    {
        try
        {
            //await AddBearerToken();
            var command = new CreateLeaveAllocationCommand
            {
                LeaveTypeId = leaveTypeId
            };
            await _client.LeaveAllocationsPOSTAsync(command);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }
}