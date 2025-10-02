using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUserService _userService;

        public CreateLeaveAllocationCommandHandler(IMapper mapper,
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userService = userService;
        }
        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Allocation", validationResult);

            var employees = await _userService.GetEmployees();
            var period = DateTime.Now.Year; 
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);   

            var leaveAllocations = new List<Domain.LeaveAllocation>();
            foreach (var emp in employees)
            {
                var allocationExists = await _leaveAllocationRepository.AllocationExists(emp.Id, request.LeaveTypeId, period);
                if (allocationExists)
                    continue;

                var allocation = new Domain.LeaveAllocation
                {
                    EmployeeId = emp.Id,
                    LeaveTypeId = request.LeaveTypeId,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period
                };
                leaveAllocations.Add(allocation);
            } 

            if (leaveAllocations.Any())
                await _leaveAllocationRepository.AddAllocations(leaveAllocations); 

            return Unit.Value;
        }
    }
}
