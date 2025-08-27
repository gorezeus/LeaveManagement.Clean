using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetAllLeaveRequestCommandHandler : IRequestHandler<GetAllLeaveRequestCommand, List<LeaveRequestDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public GetAllLeaveRequestCommandHandler(IMapper mapper, 
            ILeaveRequestRepository leaveRequestRepository)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
        }
        public async Task<List<LeaveRequestDto>> Handle(GetAllLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
            return _mapper.Map<List<LeaveRequestDto>>(leaveRequests);
        }
    }
}
