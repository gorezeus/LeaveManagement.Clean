using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //// GET: api/<LeaveRequestsController>
        [HttpGet]
        public async Task<ActionResult<IList<LeaveRequestDto>>> Get()
        {
            var leaveRequests = await _mediator.Send(new GetAllLeaveRequestCommand());
            return Ok(leaveRequests);
        }

        // GET api/<LeaveRequestsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDetailDto>> Get(int id)
        {
            var leaveRequest = await _mediator.Send(new GetLeaveRequestDetailCommand { Id = id });
            return Ok(leaveRequest);
        }

        // POST api/<LeaveRequestsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveRequestCommand leaveRequest)
        {
            var response = await _mediator.Send(leaveRequest);
            return CreatedAtAction(nameof(Get), new { id = response });
        }

        //// PUT api/<LeaveRequestsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestCommand leaveRequest)
        {
            var response = await _mediator.Send(leaveRequest);
            return NoContent();
        }

        //// DELETE api/<LeaveRequestsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteLeaveRequestCommand { Id = id });
            return NoContent();
        }

        [HttpPut]
        [Route("CancelRequest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CancelRequest(CancelLeaveRequestCommand cancelLeaveRequest)
        {
            await _mediator.Send(cancelLeaveRequest);
            return NoContent();
        }

        [HttpPut]
        [Route("UpdateApproval")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateApproval(ChangeLeaveRequestApprovalCommand updateApproval)
        {
            await _mediator.Send(updateApproval);
            return NoContent();
        }
    } 
}
