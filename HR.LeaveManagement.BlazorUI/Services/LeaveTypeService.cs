using AutoMapper;
using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services;
public class LeaveTypeService : BaseHttpService, ILeaveTypeService
{
    private readonly IMapper _mapper;

    public LeaveTypeService(IClient client, IMapper mapper, ILocalStorageService localStorage) : base(client, localStorage)
    {
        _mapper = mapper;
    }

    public async Task<List<LeaveTypeVm>> GetLeaveTypes()
    {
        await AddBearerToken();
        var leaveTypes = await _client.LeaveTypeAllAsync();
        return _mapper.Map<List<LeaveTypeVm>>(leaveTypes);
    }

    public async Task<LeaveTypeVm> GetLeaveTypeDetails(int id)
    {
        await AddBearerToken();
        var leaveType = await _client.LeaveTypeGETAsync(id);
        return _mapper.Map<LeaveTypeVm>(leaveType);
    }

    public async Task<Response<Guid>> CreateLeaveType(LeaveTypeVm leaveType)
    {
        try
        {
            await AddBearerToken();
            var createLeaveTypeCommand = _mapper.Map<CreateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypePOSTAsync(createLeaveTypeCommand);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }

    public async Task<Response<Guid>> UpdateLeaveType(int id, LeaveTypeVm leaveType)
    {
        try
        {
            await AddBearerToken();
            var updateLeaveTypeCommand = _mapper.Map<UpdateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypePUTAsync(id.ToString(), updateLeaveTypeCommand);
            return new Response<Guid>() { Success = true };
        }
        catch (ApiException e)
        {
            return ConvertApiExceptions<Guid>(e);
        }
    }

    public async Task<Response<Guid>> DeleteLeaveType(int id)
    {
        try
        {
            await AddBearerToken();
            await _client.LeaveTypeDELETEAsync(id);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException e)
        {
            return ConvertApiExceptions<Guid>(e);
        }
    }
}
