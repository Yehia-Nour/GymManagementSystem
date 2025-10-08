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
    public class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(ms => ms.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");


            builder.HasOne(ms => ms.Member)
                .WithMany(m => m.MemberSessions)
                .HasForeignKey(ms => ms.MemberId);

            builder.HasOne(ms => ms.Session)
                .WithMany(m => m.MemberSessions)
                .HasForeignKey(ms => ms.SessionId);
        }
    }
}
