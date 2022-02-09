using System.Collections.Generic;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly LeaveManagementDbContext _context;
        public LeaveAllocationRepository(LeaveManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await _context.LeaveAllocations
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return leaveAllocation;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leaveAllocations = await _context.LeaveAllocations
                .Include(x => x.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<bool> AllocationExist(string userId, int leaveTypeId, int period)
        {
            return await _context.LeaveAllocations.AnyAsync(x => x.EmployeeId == userId
                                                                 && x.LeaveTypeId == leaveTypeId
                                                                 && x.Period == period);
        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await _context.AddRangeAsync(allocations);
            await _context.SaveChangesAsync();
        }
    }
}
