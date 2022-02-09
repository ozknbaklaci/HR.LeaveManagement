using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models.LeaveRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR.LeaveManagement.MVC.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestsController(ILeaveTypeService leaveTypeService,
            ILeaveRequestService leaveRequestService)
        {
            _leaveTypeService = leaveTypeService;
            _leaveRequestService = leaveRequestService;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: LeaveRequest/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
            var model = new CreateLeaveRequestViewModel
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        //POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestViewModel leaveRequest)
        {
            if (ModelState.IsValid)
            {
                var response = await _leaveRequestService.CreateLeaveRequest(leaveRequest);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.ValidationErrors);
            }

            var leaveTypes = await _leaveTypeService.GetLeaveTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");
            leaveRequest.LeaveTypes = leaveTypeItems;

            return View(leaveRequest);
        }
    }
}
