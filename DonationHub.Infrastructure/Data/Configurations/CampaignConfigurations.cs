using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class CampaignConfigurations : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.OwnsOne(c => c.Titlle, title =>
            {
                title.Property(t => t.TitleAr).HasColumnName("TitleAr").HasMaxLength(100).IsRequired();
                title.Property(t => t.TitleEn).HasColumnName("TitleEn").HasMaxLength(100).IsRequired();
            });
            builder.OwnsOne(c => c.Description, description =>
            {
                description.Property(d => d.DescriptionAr).HasColumnName("DescriptionAr").HasMaxLength(500).IsRequired();
                description.Property(d => d.DescriptionEn).HasColumnName("DescriptionEn").HasMaxLength(500).IsRequired();
            });

            //builder.Navigation(c => c.Titlle).IsRequired();
            //builder.Navigation(c => c.Description).IsRequired();
            //builder.Navigation(c => c.GoalAmount).IsRequired();
            //builder.Navigation(c => c.RaiseAmount).IsRequired();)

            //// property for int string simple data types and navigation for complex data types
            ////builder.Property(c => c.Description).IsRequired();


            builder.OwnsOne(c => c.GoalAmount, goalAmount =>
            {
                goalAmount.Property(g => g.Amount).HasColumnName("GoalAmount").IsRequired();
                goalAmount.Property(g => g.Currency).HasColumnName("GoalCurrency").HasMaxLength(3).IsRequired();
            });
            builder.OwnsOne(c => c.RaiseAmount, raiseAmount =>
            {
                raiseAmount.Property(r => r.Amount).HasColumnName("RaiseAmount").IsRequired();
                raiseAmount.Property(r => r.Currency).HasColumnName("RaiseCurrency").HasMaxLength(3).IsRequired();
            });


            builder.HasOne(c => c.Category).WithMany().HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
            builder.HasQueryFilter(c => !c.IsDeleted);


            builder.Property(c => c.RowVersion)
           .IsRowVersion()
           .IsConcurrencyToken();

            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(c => c.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);


            builder.Property(c => c.BannerFileName).HasMaxLength(255);


            builder.Property(c => c.CreatedAt).HasDefaultValueSql("TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");




        }
    }
}
