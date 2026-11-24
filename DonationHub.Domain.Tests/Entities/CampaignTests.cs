using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using Moq;

namespace DonationHub.Domain.Tests.Entities
{
    public class CampaignTests
    {

        private static readonly DateTimeOffset now = new Mock<TimeProvider>().Object.GetUtcNow();
        private static Result<Campaign> CreateCampaign(Money? goalAmount = null, DateTimeOffset? endDate = null)
        {

            return Campaign.Create(new Title("", "Test Campaign"), new Description("", "Test Description"), goalAmount ?? Money.Create(1000, "EGP").Data, endDate ?? now.AddDays(10), null, Guid.NewGuid(), Guid.NewGuid(), now);
        }




        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(-1)]
        public void Create_WhenGoalAmountIsNotPositve_ReturnInvalidGoalAmount(decimal amount)
        {
            //Arrange

            var goalAmount = Money.Create(amount);
            // Act

            var action = CreateCampaign(goalAmount.Data);
            // Assert
            Assert.False(action.IsSuccess);
            Assert.Equal(CampaignErrors.InvalidGoalAmount, action.Error);
        }
        [Fact]
        public void Create_WhenEndDateIsInPast_ReturnEndDateInPast()
        {
            var endDate = now;

            var campaign = CreateCampaign(endDate: now);

            Assert.Equal(CampaignErrors.EndDateInPast, campaign.Error);

        }

        [Fact]
        public void Update_WhenRaisAmountGreaterThanGoalAmount_ReturnGoalBelowRaised()
        {
            var newGoalAmount = Money.Create(500).Data;

            var campaign = CreateCampaign().Data;
            campaign.UpdateRaiseAmount(Money.Create(700).Data!, now);


            var actual = campaign.Update(campaign.Title, campaign.Description, newGoalAmount, campaign.EndDate, now);
            Assert.Equal(CampaignErrors.GoalBelowRaised, actual.Error);
        }

        [Fact]
        public void Update_WhenStatusIsCompleted_ReturnNotActive()
        {
            // Arrange
            var campaign = CreateCampaign().Data!;
            campaign.UpdateRaiseAmount(Money.Create(1000).Data!, now);
            // Act
            var actual = campaign.Update(campaign.Title, campaign.Description, Money.Create(2000).Data, campaign.EndDate, now);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(CampaignErrors.NotActive, actual.Error);
        }


        [Fact]
        public void Update_WhenStatusIsExpired_ReturnNotActive()
        {
            // Arrange
            var campaign = CreateCampaign().Data!;
            campaign.UpdateRaiseAmount(Money.Create(1000).Data!, now);
            // Act
            var actual = campaign.Update(campaign.Title, campaign.Description, Money.Create(2000).Data, campaign.EndDate, now);
            campaign.Expire(now);
            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal(CampaignErrors.NotActive, actual.Error);
        }

        [Theory]
        [InlineData("2026-10-22T10:00:00+00:00")]
        [InlineData("2026-11-23T15:30:00+02:00")]
        [InlineData("2026-12-01T08:00:00-05:00")]
        public void CalculateDaysLeft_AnyDate_ReturnNotNegativeNumber(DateTimeOffset endDate)
        {
            // Arrange
            var campaign = CreateCampaign(endDate: endDate);

            //Act
            var actual = (campaign.Data.DaysLeft >= 0);
            // Assert
            Assert.True(actual);
        }
        [Theory]
        [InlineData(10000)]
        [InlineData(100)]
        [InlineData(500)]
        public void CalculateProgressPrecent_EveryNumber_ReturnPositiveNumberandAtMost100(decimal raiseAmount)
        {
            // Arrange
            var campaign = CreateCampaign().Data;
            campaign.UpdateRaiseAmount(Money.Create(raiseAmount).Data!, now);
            //Act
            var actual = campaign.ProgressPrecent;
            // Assert
            Assert.InRange(actual, 0, 100);
        }


        [Fact]
        public void CheckStatus_WhenReachedToGoalAmount_StatusBeCompleted()
        {
            // Arrange
            var campaign = CreateCampaign().Data!;
            campaign.UpdateRaiseAmount(Money.Create(1000).Data!, now);
            // Act
            var actual = campaign.Status;
            //Assert

            Assert.Equal(CampaignStatus.Completed, actual);
        }

    }
}
