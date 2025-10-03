using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail
{
    public class GetLeaveRequestDetailQueryHandler : IRequestHandler<GetLeaveRequestDetailQuery, LeaveRequestDetailDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserService _userService;

        public GetLeaveRequestDetailQueryHandler(IMapper mapper, 
            ILeaveRequestRepository leaveRequestRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            this._userService = userService;
        }
        public async Task<LeaveRequestDetailDto> Handle(GetLeaveRequestDetailQuery request, CancellationToken cancellationToken)
        {
            var leaveRequestDetail = await _leaveRequestRepository.GetLeaveRequestWithDetails(request.Id);
            var leaveRequestDto = _mapper.Map<LeaveRequestDetailDto>(leaveRequestDetail);
            leaveRequestDto.Employee = await _userService.GetEmployee(leaveRequestDetail.RequestingEmployeeId);
            return leaveRequestDto;
        }
    }
}
