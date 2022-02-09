using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models.LeaveTypes;
using Microsoft.AspNetCore.Authorization;

namespace HR.LeaveManagement.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;

        public LeaveTypesController(ILeaveTypeService leaveTypeService, ILeaveAllocationService leaveAllocationService)
        {
            _leaveTypeService = leaveTypeService;
            _leaveAllocationService = leaveAllocationService;
        }

        // GET: LeaveTypesController
        public async Task<ActionResult> Index()
        {
            var model = await _leaveTypeService.GetLeaveTypes();

            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _leaveTypeService.GetLeaveTypeDetails(id);

            return View(model);
        }

        // GET: LeaveTypesController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveTypeViewModel leaveTypeViewModel)
        {
            try
            {
                var response = await _leaveTypeService.CreateLeaveType(leaveTypeViewModel);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            return View(leaveTypeViewModel);
        }

        // GET: LeaveTypesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _leaveTypeService.GetLeaveTypeDetails(id);

            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeViewModel leaveTypeViewModel)
        {
            try
            {
                var response = await _leaveTypeService.UpdateLeaveType(leaveTypeViewModel);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(leaveTypeViewModel);
        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _leaveTypeService.DeleteLeaveType(id);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Allocate(int id)
        {
            try
            {
                var response = await _leaveAllocationService.CreateLeaveAllocations(id);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }
    }
}
