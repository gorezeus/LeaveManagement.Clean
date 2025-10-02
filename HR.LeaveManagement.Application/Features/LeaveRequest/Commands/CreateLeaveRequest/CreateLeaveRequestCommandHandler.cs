using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
{
    private readonly IMapper _mapper;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveAllocationRepository _leaveAllocationRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;
    private readonly IUserService _userService;

    public CreateLeaveRequestCommandHandler(IMapper mapper,
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveTypeRepository leaveTypeRepository,
        ILeaveAllocationRepository leaveAllocationRepository,
        IEmailSender emailSender,
        ILogger<CreateLeaveRequestCommandHandler> logger,
        IUserService userService)
    {
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _leaveAllocationRepository = leaveAllocationRepository;
        _emailSender = emailSender;
        _logger = logger;
        _userService = userService;
    }
    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestValidator(_leaveTypeRepository);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.Errors.Any())
            throw new BadRequestException("Invalid Leave Request", result);

        //TODO: Get requesting employee's id
        var userId = _userService.UserId;

        /*
         * TODO: Check on employee's allocation
         * if allocation aren't enough, return validation error with message
         */
        var allocation = await _leaveAllocationRepository.GetUserAllocations(userId, request.LeaveTypeId);
        if (allocation == null)
        { 
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                "You do not have any allocations for this leave type."));
            throw new BadRequestException("Invalid Leave Request", result);
        }

        var daysRequested = (request.EndDate - request.StartDate).TotalDays;
        if (daysRequested > allocation.NumberOfDays)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.EndDate),
                "You do not have enough days for this request."));
            throw new BadRequestException("Invalid Leave Request", result);
        }   
            
        //Create leave request
        var leaveRequestToCreate = _mapper.Map<Domain.LeaveRequest>(request);
        leaveRequestToCreate.RequestingEmployeeId = userId;
        leaveRequestToCreate.DateRequested = DateTime.Now;
        await _leaveRequestRepository.CreateAsync(leaveRequestToCreate);

        try
        {
            //send email confirmation
            var email = new EmailMessage
            {
                To = String.Empty,
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                       $"has been submitted successfully!",
                Subject = $"Leave Request Submitted!"
            };

            await _emailSender.SendEmail(email);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
        }

        return leaveRequestToCreate.Id;
    }
}
