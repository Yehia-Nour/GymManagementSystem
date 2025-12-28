using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Data.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(ms => ms.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");


            builder.HasOne(ms => ms.Member)
                .WithMany(m => m.MemberShips)
                .HasForeignKey(ms => ms.MemberId);

            builder.HasOne(ms => ms.Plan)
                .WithMany(m => m.MemberShips)
                .HasForeignKey(ms => ms.PlanId);
        }
    }
}
