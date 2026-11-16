using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.NameAr)
                    //.HasColumnName("NameAr")
                    .HasMaxLength(100)
                    .IsRequired();
                name.Property(n => n.NameEn)
                    //.HasColumnName("NameEn")
                    .HasMaxLength(100)
                    .IsRequired();
                name.HasIndex(n => new
                {
                    n.NameAr,
                    n.NameEn
                }).IsUnique();
            });
            builder.OwnsOne(c => c.Description, description =>
            {
                description.Property(d => d.DescriptionAr)
                    .HasColumnName("DescriptionAr")
                    .HasMaxLength(500);


                description.Property(d => d.DescriptionEn)
                    .HasColumnName("DescriptionEn")
                    .HasMaxLength(500);


            });
            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("TODATETIMEOFFSET(Sysutcdatetime(), '+00:00')");

            //builder.HasIndex(c => c.Name).IsUnique();


        }
    }
}
