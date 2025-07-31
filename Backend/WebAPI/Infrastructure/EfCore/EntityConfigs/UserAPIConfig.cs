using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Core.Entities;

namespace WebAPI.Infrastructure.EfCore.EntityConfigs
{
    public class UserAPIConfig : IEntityTypeConfiguration<UserAPI>
    {
        public void Configure(EntityTypeBuilder<UserAPI> builder)
        {
            builder.HasOne(p => p.Role)
                .WithMany(s => s.ApiUsers)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
