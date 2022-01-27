﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeViewModel>> GetLeaveTypes();
        Task<LeaveTypeViewModel> GetLeaveTypeDetails(int id);
        Task<Response<int>> CreateLeaveType(CreateLeaveTypeViewModel leaveTypeViewModel);
        Task<Response<int>> UpdateLeaveType(LeaveTypeViewModel leaveTypeViewModel);
        Task<Response<int>> DeleteLeaveType(int id);
    }
}
