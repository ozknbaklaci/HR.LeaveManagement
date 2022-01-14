using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands
{
    public class CreateLeaveRequestCommand : IRequest<Unit>
    {
        public CreateLeaveRequestDto LeaveRequestDto { get; set; }
    }
}
