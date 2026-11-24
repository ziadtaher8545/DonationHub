using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Authentication.ChangePassword;
using DonationHub.Application.Features.Authentication.GetUser;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.RefreshToken;
using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Application.Features.Authentication.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{
    public class AuthenticationController : BaseController
    {

        public AuthenticationController(IMediator imediator) : base(imediator)
        {

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return ToActionResult(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return ToActionResult(result);
        }

        [HttpGet("AuthMe"), Authorize]

        public async Task<ActionResult<UserDataDto>> AuthMe()
        {
            var email = GetEmailByToken();
            var getUserQuery = new GetUserQuery(email);
            var user = await _mediator.Send(getUserQuery);
            return ToActionResult(user);
        }

        [HttpPost("refresh")]

        public async Task<ActionResult<string>> Refresh(RefreshTokenCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            return ToActionResult(result);
        }

        // Bonus
        [HttpPut("ChangePaswword")]

        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwordDto, CancellationToken ct)
        {
            var email = GetEmailByToken();
            var result = await _mediator.Send(new ChangePasswordCommand(passwordDto.CurrentPassword, passwordDto.NewPassword, passwordDto.ConfirmedNewPassword, email));
            return ToActionResult(result);
        }
        [HttpPut("UpdateProfile"), Authorize]
        public async Task<ActionResult<UserDataDto>> UpdateProfile(UpdateProfileCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            return ToActionResult(result);
        }


    }
}
