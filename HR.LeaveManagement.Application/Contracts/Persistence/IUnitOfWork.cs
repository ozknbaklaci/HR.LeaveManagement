﻿using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        ILeaveAllocationRepository LeaveAllocationRepository { get; }
        ILeaveRequestRepository LeaveRequestRepository { get; }
        ILeaveTypeRepository LeaveTypeRepository { get; }
        Task Save();
    }
}
