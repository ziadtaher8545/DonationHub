
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Enums;
using DonationHub.Domain.Interfaces;
using DonationHub.Domain.ValueObjects;

namespace DonationHub.Domain.Entities
{
    public class Campaign : BaseEntity, ISoftDeletable
    {
        public Title Title { get; private set; }
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

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public ICollection<Donation> Donations { get; set; }

        // calculaction Properties 
        public int ProgressPrecent
        {
            get => Math.Min(100, (int)decimal.Round(RaiseAmount.Amount / GoalAmount.Amount * 100));
        }

        public int DaysLeft { get => GetDaysLeft(TimeProvider.System); }

        private int GetDaysLeft(TimeProvider timeProvider)
        {
            return Math.Max(0, (int)Math.Ceiling((EndDate - timeProvider.GetUtcNow()).TotalDays));
        }


        //private Campaign() { }

        public static Result<Campaign> Create(Title title, Description? description, Money goalAmount, DateTimeOffset endDate, string? bannerFileName, Guid categoryId, Guid createdByUserId, DateTimeOffset now)
        {
            if (goalAmount.Amount <= 0)
                return CampaignErrors.InvalidGoalAmount;

            if (endDate <= now)
                return CampaignErrors.EndDateInPast;

            if (categoryId == Guid.Empty || createdByUserId == Guid.Empty)
                return CampaignErrors.NotFound;

            var resultMoney = Money.Create(0);
            if (!resultMoney.IsSuccess)
                return resultMoney.Error;
            return new Campaign
            {
                Title = title,
                Description = description,
                GoalAmount = goalAmount,
                RaiseAmount = resultMoney.Data!,
                EndDate = endDate,
                BannerFileName = bannerFileName,
                Status = CampaignStatus.Active,
                CategoryId = categoryId,
                CreatedByUserId = createdByUserId
            };
        }

        public Result ChangeEndDate(DateTimeOffset newEndDate, DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
                return CampaignErrors.NotActive;

            if (newEndDate <= now)
                return CampaignErrors.EndDateInPast;

            EndDate = newEndDate;
            UpdatedAt = now;

            return Result.Ok();
        }

        public Result EnsureAcceptingDonation(DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
                return CampaignErrors.NotActive;

            if (EndDate <= now)
                return CampaignErrors.EndDateInPast;

            return Result.Ok();
        }

        public Result ChangeGoalAmount(Money newGoalAmount, DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
                return CampaignErrors.NotActive;

            if (newGoalAmount.Amount <= 0)
                return CampaignErrors.InvalidGoalAmount;

            if (newGoalAmount.Amount < RaiseAmount.Amount)
                return CampaignErrors.GoalBelowRaised;

            GoalAmount = newGoalAmount;
            UpdatedAt = now;

            return Result.Ok();
        }

        public Result UpdateRaiseAmount(Money newRaiseAmount, DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
                return CampaignErrors.NotActive;
            if (newRaiseAmount.Amount <= 0)
            {
                return CampaignErrors.InvalidRaiseAmount;
            }
            if (RaiseAmount.Amount >= GoalAmount.Amount)
                return CampaignErrors.GoalBelowRaised;


            RaiseAmount = newRaiseAmount;
            UpdatedAt = now;

            if (RaiseAmount.Amount >= GoalAmount.Amount)
            {
                Status = CampaignStatus.Completed;
            }

            return Result.Ok();
        }

        public void Delete(DateTimeOffset now)
        {
            IsDeleted = true;
            DeletedAt = now;
            UpdatedAt = now;
        }

        public void Restore(DateTimeOffset now)
        {
            IsDeleted = false;
            DeletedAt = null;
            UpdatedAt = now;
        }


        public Result Update(Title title, Description description, Money goalAmount, DateTimeOffset endDate, DateTimeOffset now)
        {
            if (Status != CampaignStatus.Active)
                return CampaignErrors.NotActive;

            if (endDate <= now)
                return CampaignErrors.EndDateInPast;

            if (goalAmount.Currency != GoalAmount.Currency)
                return CampaignErrors.InvalidGoalAmount;

            if (goalAmount.Amount < RaiseAmount.Amount)
                return CampaignErrors.GoalBelowRaised;

            Title = title;
            Description = description;
            GoalAmount = goalAmount;
            EndDate = endDate;
            return Result.Ok();
        }

        public void Expire(DateTimeOffset now)
        {
            UpdatedAt = now;
            Status = CampaignStatus.Expired;
        }



        public Result Cancel(DateTimeOffset now)
        {
            if (Status == CampaignStatus.Completed)
                return CampaignErrors.CannotCancelCompleted;

            if (Status == CampaignStatus.Expired)
                return CampaignErrors.CannotCancelExpired;

            if (Status == CampaignStatus.Cancelled)
                return CampaignErrors.AlreadyCancelled;

            Status = CampaignStatus.Cancelled;
            UpdatedAt = now;

            return Result.Ok();
        }
    }
}
