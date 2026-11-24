using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class PaymentAttemptConfigurations : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.Property(pa => pa.CreatedAt).HasDefaultValueSql("TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");
            builder.HasOne(p => p.Donation)
                    .WithMany()
                    .HasForeignKey(p => p.DonationId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
