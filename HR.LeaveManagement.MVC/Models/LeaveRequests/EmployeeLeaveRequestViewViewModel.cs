using System.Collections.Generic;
using HR.LeaveManagement.MVC.Models.LeaveAllocations;

namespace HR.LeaveManagement.MVC.Models.LeaveRequests
{
    public class EmployeeLeaveRequestViewViewModel
    {
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
        public List<LeaveRequestViewModel> LeaveRequests { get; set; }
    }
}
