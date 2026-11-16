using DonationHub.Domain.Enums;
using DonationHub.Domain.Interfaces;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class Campaign : BaseEntity, ISoftDeletable
    {
        public Title Titlle { get; private set; }
        public Description? Description { get; private set; }
        public Money GoalAmount { get; private set; }
        public Money RaiseAmount { get; private set; }

        public DateTimeOffset EndDate { get; private set; }
        public string? BannerFileName { get; private set; }
        public CampaignStatus Status { get; private set; }

        public byte[] RowVersion { get; private set; } = [];




        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public bool IsDeleted { get;  set; }

        public DateTimeOffset? DeletedAt { get;  set; }

        public static Campaign Create(Title title, Description? description, Money goalAmount, DateTimeOffset endDate, string? bannerFileName, Guid categoryId, Guid createdByUserId)
        {

            if (endDate <= DateTimeOffset.UtcNow)
            {
                throw new ArgumentException("End date must be in the future.", nameof(endDate));
            }
            return new Campaign
            {
                Titlle = title,
                Description = description,
                GoalAmount = goalAmount,
                RaiseAmount = new Money(0),
                EndDate = endDate,
                BannerFileName = bannerFileName,
                Status = CampaignStatus.Active,
                CategoryId = categoryId,
                CreatedByUserId = createdByUserId
            };
        }
        public void ChangeEndDate(DateTimeOffset newEndDate)
        {
            if (newEndDate <= DateTimeOffset.UtcNow)
            {
                throw new ArgumentException("End date must be in the future.", nameof(newEndDate));
            }
            EndDate = newEndDate;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
        public void EnshureAcceptingDonation()
        {
            if (Status != CampaignStatus.Active || EndDate <= DateTimeOffset.UtcNow)
            {
                throw new InvalidOperationException("Campaign is not accepting donations.");

            }
        }

        public void ChangeGoalAmount(Money newGoalAmount)
        {
            if (newGoalAmount.Amount <= 0)
            {
                throw new ArgumentException("Goal amount must be greater than zero.", nameof(newGoalAmount));
            }
            if (newGoalAmount.Amount < RaiseAmount.Amount)
            {
                throw new ArgumentException("New goal amount cannot be less than the raised amount.", nameof(newGoalAmount));
            }
            GoalAmount = newGoalAmount;
            UpdatedAt = DateTimeOffset.UtcNow;
        }


        public void UpdateRaiseAmount(Money newRaiseAmount)
        {
            if (newRaiseAmount.Amount < 0)
            {
                throw new ArgumentException("Raise amount cannot be negative.", nameof(newRaiseAmount));
            }
            if (newRaiseAmount.Amount > GoalAmount.Amount)
            {
                throw new ArgumentException("Raise amount cannot exceed the goal amount.", nameof(newRaiseAmount));
            }

            RaiseAmount = newRaiseAmount;
            UpdatedAt = DateTimeOffset.UtcNow;
            if (RaiseAmount.Amount >= GoalAmount.Amount)
            {
                Status = CampaignStatus.Completed;
            }
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }

}
