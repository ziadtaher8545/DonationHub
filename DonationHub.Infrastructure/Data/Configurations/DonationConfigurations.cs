using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class DonationConfigurations : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.Property(d => d.CreatedAt).HasDefaultValueSql("TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");

            builder.OwnsOne(d => d.Amount, amount =>
            {

                amount.Property(am => am.Amount).HasColumnType("decimal(18,2)");
                amount.Property(am => am.Currency).HasMaxLength(3);

            });
            builder.Ignore(d => d.UpdatedAt);
            //builder.Property(d => d.UpdatedAt).HasColumnName("EndDate");

            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(d => d.DonorUserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Campaign).WithMany().HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
