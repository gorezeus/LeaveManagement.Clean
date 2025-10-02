using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetAllLeaveRequestQuery : IRequest<List<LeaveRequestDto>>
    {
        public bool IsLoggedInUser { get; set; }
    }
}
