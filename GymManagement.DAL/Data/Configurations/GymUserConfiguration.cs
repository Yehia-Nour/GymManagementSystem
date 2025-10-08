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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(g => g.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(g => g.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(g => g.Phone)
                .HasMaxLength(11);

            builder.HasIndex(g => g.Email).IsUnique();
            builder.HasIndex(g => g.Phone).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone Like '01%' And Phone Like '%[0-9]%'");
            });

            builder.OwnsOne(g => g.Address, a =>
            {
                a.Property(p => p.BuildingNumber)
                    .HasColumnName("BuildingNumber")
                    .HasMaxLength(10);

                a.Property(p => p.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(100);

                a.Property(p => p.City)
                    .HasColumnName("City")
                    .HasMaxLength(50);
            });
        }
    }
}
