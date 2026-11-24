using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Tests.Features.Authentication.Handlers
{
    public class LoginHandlerTest
    {
        private readonly Mock<IIdentityService> _identityService;
        private readonly Mock<ITokenService> _tokenServiece;
        private readonly LoginHandler _handler;

        public LoginHandlerTest()
        {
            _identityService = new();
            _tokenServiece = new();
            _handler = new(_identityService.Object, _tokenServiece.Object);
        }
        private static LoginCommand CreateValidCommand()
        {
            return new LoginCommand("test@gmail.com", "P@ssw0rd");


        }
        [Fact]
        public async Task HandleLogin_WhenEmailOrPasswordIsWrong_ReturnInvalidCredentials()
        {
            // Arrange
            var cmd = CreateValidCommand();
            _identityService.Setup(i => i.LoginAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result<UserDto>.Fail(AuthErrors.InvalidCredentials));
            // Act
            var actual = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            actual.IsSuccess.Should().BeFalse();
            actual.Error.Should().Be(AuthErrors.InvalidCredentials);
        }
    }
}
