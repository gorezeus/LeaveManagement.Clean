using AutoMapper;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LeaveTypeDetailsDto, LeaveTypeVm>().ReverseMap();
            CreateMap<LeaveTypeVm, CreateLeaveTypeCommand>();
            CreateMap<LeaveTypeVm, UpdateLeaveTypeCommand>();
        }
    }
}
