using AutoMapper;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequest;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LeaveTypeDto, LeaveTypeVm>().ReverseMap();
            CreateMap<LeaveTypeDetailsDto, LeaveTypeVm>().ReverseMap();
            CreateMap<LeaveTypeVm, CreateLeaveTypeCommand>().ReverseMap();
            CreateMap<LeaveTypeVm, UpdateLeaveTypeCommand>().ReverseMap();

            CreateMap<CreateLeaveRequestCommand, LeaveRequestVm>().ReverseMap();
            //CreateMap<LeaveTypeDto, LeaveTypeVm>().ReverseMap();
            //CreateMap<LeaveTypeDetailsDto, LeaveTypeVm>().ReverseMap();
            //CreateMap<CreateLeaveTypeCommand, LeaveTypeVm>().ReverseMap();
            //CreateMap<UpdateLeaveTypeCommand, LeaveTypeVm>().ReverseMap();
        }
    }
}
