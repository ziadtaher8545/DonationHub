using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.Login
{
    internal class LoginHandler : IRequestHandler<LoginCommand, Result<UserDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        //private readonly IValidator<LoginCommand> _validator;
        public LoginHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            //_validator = validator;
        }

        public async Task<Result<UserDto>> Handle(LoginCommand command, CancellationToken ct)
        {
            //var validationResult = await _validator.ValidateAsync(command, ct);
            //if (!validationResult.IsValid)
            //{
            //    // shoud return result 
            //    return AuthErrors.InvalidCredentials;
            //}
            var user = await _identityService.LoginAsync(command, ct);
            return user;
        }

    }
}
