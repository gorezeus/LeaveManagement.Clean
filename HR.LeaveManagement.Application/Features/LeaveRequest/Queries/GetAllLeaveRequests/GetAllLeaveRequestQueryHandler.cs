using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetAllLeaveRequestQueryHandler : IRequestHandler<GetAllLeaveRequestQuery, List<LeaveRequestDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserService _userService;

        public GetAllLeaveRequestQueryHandler(IMapper mapper, 
            ILeaveRequestRepository leaveRequestRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _userService = userService;
        }
        public async Task<List<LeaveRequestDto>> Handle(GetAllLeaveRequestQuery request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<Domain.LeaveRequest>();
            var requests = new List<LeaveRequestDto>();

            if (request.IsLoggedInUser)
            {
                var userId = _userService.UserId;
                leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId);

                var employee = await _userService.GetEmployee(userId);
                requests = _mapper.Map<List<LeaveRequestDto>>(leaveRequests);

                foreach (var lr in requests)
                    lr.Employee = employee;
            }
            else
            {
                leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestDto>>(leaveRequests);
                foreach (var lr in requests)
                    lr.Employee = await _userService.GetEmployee(lr.RequestingEmployeeId);
            }
            return requests;
        }
    }
}
