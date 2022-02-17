using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Models.LeaveRequests;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveRequestService
    {
        Task<AdminLeaveRequestViewModel> GetAdminLeaveRequestList();
        Task<EmployeeLeaveRequestViewViewModel> GetUserLeaveRequest();
        Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestViewModel leaveRequestViewModel);
        Task<LeaveRequestViewModel> GetLeaveRequest(int id);
        Task DeleteLeaveRequest(int id);
        Task ApproveLeaveRequest(int id, bool approved);
    }
}
