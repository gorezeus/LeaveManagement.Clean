using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetail
{
    public class GetLeaveAllocationDetailQueryHandler : IRequestHandler<GetLeaveAllocationDetailQuery, LeaveAllocationDetailDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public GetLeaveAllocationDetailQueryHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
        }
        public async Task<LeaveAllocationDetailDto> Handle(GetLeaveAllocationDetailQuery request, CancellationToken cancellationToken)
        {
            var leaveAllocationDetail = await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);

            if (leaveAllocationDetail == null)
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);

            return _mapper.Map<LeaveAllocationDetailDto>(leaveAllocationDetail);
        }
    }
}
