using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.RefreshToken
{
    internal class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, UserDto>
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IIdentityService _identityServiece;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenHandler(ITokenService tokenService, ITokenRepository tokenRepository, IIdentityService identityService, IIdentityService identityServiece, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _identityServiece = identityServiece;
            _unitOfWork = unitOfWork;
        }

        //private re
        public async Task<UserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new InvalidOperationException();

            var tokenHash = _tokenService.HashToken(request.RefreshToken);
            var oldToken = await _tokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);
            if (oldToken is null)
            {
                throw new UnauthorizedAccessException();
            }
            // Bonus
            if (oldToken.RevokedAt.HasValue)
            {
                // Revoke all 
                throw new UnauthorizedAccessException();

            }

            var user = await _identityServiece.GetUserByIdAsync(oldToken.UserId);
            if (user is null)
                throw new UnauthorizedAccessException();

            var accessToken = _tokenService.CreateToken(user.Id.ToString(), user.Email, user.UserName, user.Roles);
            var (plaintext, newRefreshToken) = _tokenService.CreateRefreshToken(oldToken.UserId);
            await _tokenRepository.CreateAsync(newRefreshToken, cancellationToken);


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            oldToken.Revoke(newRefreshToken.Id);

            return new UserDto
            {
                Email = user.Email!,
                FullName = user.FullName!,
                AcessToken = accessToken,
                RefershToken = plaintext
            };



        }


    }
}
