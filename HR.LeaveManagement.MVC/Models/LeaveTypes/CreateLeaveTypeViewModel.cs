using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models.LeaveTypes
{
    public class CreateLeaveTypeViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Default Number Of Days")]
        public int DefaultDays { get; set; }

    }
}
