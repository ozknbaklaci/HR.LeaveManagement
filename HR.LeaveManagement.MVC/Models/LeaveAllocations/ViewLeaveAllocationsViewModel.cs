using System.Collections.Generic;

namespace HR.LeaveManagement.MVC.Models.LeaveAllocations
{
    public class ViewLeaveAllocationsViewModel
    {
        public string EmployeeId { get; set; }
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
    }
}
