using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                RoleId = "571e2784-01f2-4eaf-8f64-4f8a7077bdff",
                UserId = "b2197eca-5667-43be-a7ef-8fd737efd2c1"
            }, new IdentityUserRole<string>
            {
                RoleId = "04ce4467-7ce1-49c6-820c-efa4621eaf33",
                UserId = "e2adf677-15a5-49a9-a1d4-b8cf3ba12a8d"
            });
        }
    }
}
