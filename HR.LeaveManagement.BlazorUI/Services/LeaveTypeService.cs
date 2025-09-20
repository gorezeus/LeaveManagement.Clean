using AutoMapper;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services;
public class LeaveTypeService : BaseHttpService, ILeaveTypeService
{
    private readonly IMapper _mapper;

    public LeaveTypeService(IClient client, IMapper mapper) : base(client)
    {
        _mapper = mapper;
    }

    public async Task<List<LeaveTypeVm>> GetLeaveTypes()
    {
        var leaveTypes = await _client.LeaveTypeAllAsync();
        return _mapper.Map<List<LeaveTypeVm>>(leaveTypes);
    }

    public async Task<LeaveTypeVm> GetLeaveTypeDetails(int id)
    {
        var leaveType = await _client.LeaveTypeGETAsync(id);
        return _mapper.Map<LeaveTypeVm>(leaveType);
    }

    public async Task<Response<Guid>> CreateLeaveType(LeaveTypeVm leaveType)
    {
        try
        {
            var createLeaveTypeCommand = _mapper.Map<CreateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypePOSTAsync(createLeaveTypeCommand);
            return new Response<Guid>
            {
                Success = true
            };
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
            await _client.LeaveTypeDELETEAsync(id);
            return new Response<Guid> { Success = true };
        }
        catch (ApiException e)
        {
            return ConvertApiExceptions<Guid>(e);
        }
    }
}
