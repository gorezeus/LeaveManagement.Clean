using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
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
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;

    public CreateLeaveRequestCommandHandler(IMapper mapper, 
        ILeaveRequestRepository leaveRequestRepository, 
        ILeaveTypeRepository leaveTypeRepository, 
        IEmailSender emailSender, 
        ILogger<CreateLeaveRequestCommandHandler> logger)
    {
        _mapper = mapper;
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _emailSender = emailSender;
        _logger = logger;
    }
    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestValidator(_leaveTypeRepository);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.Errors.Any())
            throw new BadRequestException("Invalid Leave Request", result);
        //TODO: Get requesting employee's id

        /*
         * TODO: Check on employee's allocation
         * if allocation aren't enough, return validation error with message
         */

        //Create leave request
        var leaveRequestToCreate = _mapper.Map<Domain.LeaveRequest>(request);
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
