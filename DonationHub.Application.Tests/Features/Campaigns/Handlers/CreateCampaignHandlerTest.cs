using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Campaigns.CreateCampaign;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Tests.Features.Campaigns.Handlers
{
    public class CreateCampaignHandlerTest
    {
        private readonly Mock<IIdentityService> _identityService = new();
        private static readonly Mock<TimeProvider> _timeProvider = new();
        private readonly CreateCampaignHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private readonly Mock<ICampaignRepository> _campaignRepository = new();
        private static CreateCampaignCommand CreateValidCreateCampaignCommand()
        {
            return new CreateCampaignCommand("Test", "Test", "Test", "Test", 1000, "EGP", null, _timeProvider.Object.GetUtcNow().AddDays(10), Guid.NewGuid(), "test@gmail.com");
        }
        private static UserDataDto CreateValidUserDataDto()
        {
            return new UserDataDto(Guid.NewGuid(), "test@gmail.com", "Test", "TestTest", ["Admin"]);
        }
        public CreateCampaignHandlerTest()
        {
            _handler = new(_campaignRepository.Object, _timeProvider.Object, _identityService.Object, _unitOfWork.Object);
        }
        [Fact]
        public async Task HandleCreateCampaign_WhenCategoryIsNotFount_ReturnNotFound()
        {
            var userDataDto = CreateValidUserDataDto();
            // Arrange
            var command = CreateValidCreateCampaignCommand();
            _identityService.Setup(i => i.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<UserDataDto>.Ok(userDataDto));
            command.CategoryId = Guid.Empty;

            // Act
            var actual = await _handler.Handle(command, CancellationToken.None);

            // Assert
            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be(CampaignErrors.NotFound);


        }

        [Fact]
        public async Task HandleCreateCampaign_WhenSucceed_ReturnOk()
        {
            var userDataDto = CreateValidUserDataDto();
            // Arrange
            var command = CreateValidCreateCampaignCommand();
            _identityService.Setup(i => i.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<UserDataDto>.Ok(userDataDto));

            _campaignRepository.Setup(c => c.CreateAsync(It.IsAny<Campaign>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

            // Act
            var actual = await _handler.Handle(command, CancellationToken.None);

            // Assert
            actual.IsSuccess.Should().BeTrue();
            actual.Error.Should().BeNull();


        }

    }
}
