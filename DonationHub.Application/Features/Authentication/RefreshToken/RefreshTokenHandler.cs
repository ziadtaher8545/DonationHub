
using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Authentication.RefreshToken;

internal class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<string>>
{
    private readonly ITokenService _tokenService;


    public RefreshTokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;

    }

    public async Task<Result<string>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {

        return await _tokenService.ActivateRefreshToken(request.RefreshToken, cancellationToken);

    }
}
