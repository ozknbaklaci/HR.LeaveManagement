using System;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Persistence;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LeaveManagementDbContext _dbContext;
        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveTypeRepository _leaveTypeRepository;
        private ILeaveRequestRepository _leaveRequestRepository;


        public UnitOfWork(LeaveManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILeaveAllocationRepository LeaveAllocationRepository =>
            _leaveAllocationRepository ??= new LeaveAllocationRepository(_dbContext);

        public ILeaveRequestRepository LeaveRequestRepository =>
            _leaveRequestRepository ??= new LeaveRequestRepository(_dbContext);

        public ILeaveTypeRepository LeaveTypeRepository =>
            _leaveTypeRepository ??= new LeaveTypeRepository(_dbContext);

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
