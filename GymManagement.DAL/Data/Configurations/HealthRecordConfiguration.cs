using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Data.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                .HasKey(h => h.Id);

            builder.HasOne<Member>()
                .WithOne(m => m.HealthRecord)
                .HasForeignKey<HealthRecord>(h => h.Id);

            builder.Ignore(h => h.CreatedAt);
            builder.Ignore(h => h.UpdatedAt);
        }
    }
}
