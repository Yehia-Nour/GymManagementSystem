using GymManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 And 25");
                tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            builder.HasOne(s => s.Category)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CategoryId);

            builder.HasOne(s => s.Trainer)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.TrainerId);
        }
    }
}
