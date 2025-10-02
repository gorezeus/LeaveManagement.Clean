using AutoMapper;
using HR.LeaveManagement.BlazorUI.Models;
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

            CreateMap<LeaveRequestDto, LeaveRequestVm>()
                .ForMember(q => q.DateRequested, opt => opt.MapFrom(x => x.DateRequested.DateTime))
                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<LeaveRequestDetailDto, LeaveRequestVm>()
                .ForMember(q => q.DateRequested, opt => opt.MapFrom(x => x.DateRequested.DateTime))
                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<CreateLeaveRequestCommand, LeaveRequestVm>().ReverseMap();
            CreateMap<UpdateLeaveRequestCommand, LeaveRequestVm>().ReverseMap();

            CreateMap<EmployeeVm, Employee>().ReverseMap();

            //CreateMap<LeaveTypeDto, LeaveTypeVm>().ReverseMap();
            //CreateMap<LeaveTypeDetailsDto, LeaveTypeVm>().ReverseMap();
            //CreateMap<CreateLeaveTypeCommand, LeaveTypeVm>().ReverseMap();
            //CreateMap<UpdateLeaveTypeCommand, LeaveTypeVm>().ReverseMap();
        }
    }
}
