using System;
using System.Linq;
using System.Threading.Tasks;
using HR.LeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence
{
    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual async Task<int> SaveChangesAsync(string userName = "SYSTEM")
        {
            var entityEntries = base.ChangeTracker.Entries<BaseDomainEntity>()
                .Where(x => x.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in entityEntries)
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
                entry.Entity.LastModifiedBy = userName;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                    entry.Entity.CreatedBy = userName;
                }
            }

            var result = await base.SaveChangesAsync();

            return result;
        }
    }
}
