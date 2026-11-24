using DonationHub.Application.Features.Campaigns.CreateCampaign;
using DonationHub.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DonationHub.Application.Tests.Features.Campaigns.Validators
{

    public class CreateCampaignValidatorTest
    {
        private readonly CreateCamaignCommandValidator _validator = new();
        private static readonly Mock<TimeProvider> _timeProvider = new();
        //public readonly DateTimeOffset now = _timeProvider.Object.GetLocalNow();

        private static CreateCampaignCommand CreateValidCommand()
        {
            return new CreateCampaignCommand("Test", "Test", "Test", "TEst", 1000, "EGP", null, _timeProvider.Object.GetUtcNow(), Guid.NewGuid(), "test@gmail.com");


        }





        [Theory]
        [InlineData("", "Test")]
        [InlineData("Test", "")]
        [InlineData("", "")]

        public void Validate_WhenTitlebilingualMissed_ReturnValidationError(string titleAr, string titleEn)
        {
            // Arrange
            var command = CreateValidCommand();
            command.TitleAr = titleAr;
            command.TitleEn = titleEn;

            // Act 
            var actual = _validator.TestValidate(command);


            // Assert
            if (string.IsNullOrWhiteSpace(titleAr))
                actual.ShouldHaveValidationErrorFor(x => x.TitleAr);

            if (string.IsNullOrWhiteSpace(titleEn))
                actual.ShouldHaveValidationErrorFor(x => x.TitleEn);



        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Validate_WhenGoalAmountIsNotPositive_ReturnValidationError(decimal goalamount)
        {
            // Arrange
            var command = CreateValidCommand();
            command.GoalAmount = goalamount;

            // Act 
            var actual = _validator.TestValidate(command);


            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.GoalAmount);


        }

        [Fact]
        public void Validate_WhenCategoryIdBeEmpty_ReturnValidationError()
        {
            // Arrange
            var command = CreateValidCommand();
            command.CategoryId = Guid.Empty;

            // Act 
            var actual = _validator.TestValidate(command);


            // Assert
            actual.ShouldHaveValidationErrorFor(x => x.CategoryId);



        }




    }
}
