using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Campaigns.UpdateCampaign;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Application.Specifications;
using DonationHub.Application.Specifications.Category;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Tests.Features.Campaigns.Handlers
{
    public class UpdateCampaignHandlerTest
    {
        private readonly UpdateCampaignHandler _handler;
        private readonly Mock<ICampaignRepository> _campaignRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private static readonly Mock<TimeProvider> _timeProvider = new();
        private readonly Mock<IIdentityService> _identityService;
        private static DateTimeOffset now = _timeProvider.Object.GetUtcNow();

        public UpdateCampaignHandlerTest()
        {
            _identityService = new();
            _campaignRepository = new();
            _unitOfWork = new();

            _handler = new(_campaignRepository.Object, _unitOfWork.Object, _timeProvider.Object, _identityService.Object);
        }
        private static UpdateCampaignCommand CreateValidCommand()
        {
            return new UpdateCampaignCommand(Guid.NewGuid(), "Test", "Test", "Test", "Test", 10, "EGP", _timeProvider.Object.GetUtcNow().AddDays(10), null, "AAA555");
        }
        private static UserDataDto CreateValidUser() => new UserDataDto(Guid.NewGuid(), "test@gmail.com", "TEst", "TEST", []);
        private static Campaign CreateValidCampaign() =>
             Campaign.Create(new Title("", "Test Campaign"), new Description("", "Test Description"), Money.Create(1000, "EGP").Data, now.AddDays(10), null, Guid.NewGuid(), Guid.NewGuid(), now).Data;
        [Fact]
        public async Task HandleUpdateCampaign_WhenIdIsUnKnown_ReturnNotFound()
        {
            var command = CreateValidCommand();
            // Arrange
            //var spec = new Mock<CampaignSpecification>();
            _campaignRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CampaignSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<Campaign>.Fail(CampaignErrors.NotFound));
            // Act
            var actual = await _handler.Handle(command, CancellationToken.None);
            // Assert
            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be(CampaignErrors.NotFound);



        }



        [Fact]
        public async Task HandleUpdateCampaign_WhenConcurrencyConflictOccurs_ReturnsConcurrencyError()
        {
            // Arrange
            var cmd = CreateValidCommand();

            var campaign = CreateValidCampaign();

            var user = CreateValidUser();

            _campaignRepository.Setup(r => r.GetByIdAsync(cmd.Id, It.IsAny<CampaignSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<Campaign>.Ok(campaign));

            _identityService.Setup(s => s.GetUserByIdAsync(campaign.CreatedByUserId, It.IsAny<CancellationToken>())).ReturnsAsync(Result<UserDataDto>.Ok(user));

            _campaignRepository.Setup(r => r.MakeCheckRowVersion(campaign, cmd.RowVersion)).Returns(Result.Ok());

            _campaignRepository.Setup(r => r.UpdateAsync(campaign)).ReturnsAsync(Result.Ok());

            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateConcurrencyException());

            // Act
            var result = await _handler.Handle(
                cmd,
                CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(CampaignErrors.ConcurrencyConflict, result.Error);

            _unitOfWork.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
