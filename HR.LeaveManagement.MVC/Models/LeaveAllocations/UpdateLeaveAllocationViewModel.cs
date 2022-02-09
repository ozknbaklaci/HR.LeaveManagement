using System.ComponentModel.DataAnnotations;
using HR.LeaveManagement.MVC.Models.LeaveTypes;

namespace HR.LeaveManagement.MVC.Models.LeaveAllocations
{
    public class UpdateLeaveAllocationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        [Range(1, 50, ErrorMessage = "Enter Valid Number")]
        public int NumberOfDays { get; set; }
        public LeaveTypeViewModel LeaveType { get; set; }
    }
}
