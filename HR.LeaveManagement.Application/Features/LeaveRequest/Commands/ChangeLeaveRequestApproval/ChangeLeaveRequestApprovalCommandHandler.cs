using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ChangeLeaveRequestApprovalCommand> _logger;

    public ChangeLeaveRequestApprovalCommandHandler(ILeaveRequestRepository leaveRequestRepository,
        IEmailSender emailSender,
        ILogger<ChangeLeaveRequestApprovalCommand> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _emailSender = emailSender;
        _logger = logger;
    }
    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var validator = new ChangeLeaveRequestApprovalValidator();
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.Errors.Any())
            throw new BadRequestException("Invalid Approval status", result);

        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.LeaveRequestId);
        
        if (leaveRequest == null)
            throw new NotFoundException(nameof(leaveRequest), request.LeaveRequestId);

        leaveRequest.Approved = request.Approved;

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        // if request is approved, get and update the employee's allocations

        try
        {
            var email = new EmailMessage
            {
                To = String.Empty,
                Body = $"Your leave request for {leaveRequest.StartDate} to {leaveRequest.EndDate:D} " +
                       $"has been cancelled successfully.",
                Subject = "Leave Request Cancelled"
            };

            //await _emailSender.SendEmail(email);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
        }
        
        return Unit.Value;
    }
}
