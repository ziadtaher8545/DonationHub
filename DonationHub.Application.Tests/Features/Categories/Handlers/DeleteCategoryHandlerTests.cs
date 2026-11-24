using DonationHub.Application.Features.Categories.DeleteCategory;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Tests.Features.Categories.Handlers
{
    public class DeleteCategoryHandlerTests
    {
        
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly DeleteCategoryHandler _handler;
        public DeleteCategoryHandlerTests()
        {
            _categoryRepository = new();
            _unitOfWork = new();

            _handler = new(_categoryRepository.Object, _unitOfWork.Object);
        }


        [Fact]

        async Task HandleDelete_WhenCategoryHasCampaigns_RturnHasCampaigns()
        {
            // Arrange

            var category = Category.Create(new Name("Test", "Test"), null);
            var command = new DeleteCategoryCommand(category.Id);




            _categoryRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<Category>.Ok(category));



            _categoryRepository.Setup(x => x.HasCampaignsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var actual = await _handler.Handle(command, CancellationToken.None);
            // Assert

            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be(CategoryErrors.HasCampaigns);


    

        }
    }
}
