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
        public async Task<IActionResult> Register(RegisterCommand command, CancellationToken ct)
        {
            var result = await _imediator.Send(command, ct);
            //if (!result )
            //    return 
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
        {
            var result = await _imediator.Send(command, ct);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("AuthMe"), Authorize]

        public async Task<IActionResult> AuthMe()
        {
            var email = GetEmailByToken();
            var getUserQuery = new GetUserQuery(email);
            var user = await _imediator.Send(getUserQuery);
            if (user is null) return Unauthorized(user);
            //Console.WriteLine(user.Email);
            return Ok(user);
        }

        [HttpPost("refresh")]

        public async Task<IActionResult> Refresh(RefreshTokenCommand command, CancellationToken ct)
        {
            var result = await _imediator.Send(command, ct);

            return Ok(result);
        }

        // Bonus
        [HttpPut("ChangePaswword")]

        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto, CancellationToken ct)
        {
            var email = GetEmailByToken();
            var result = await _imediator.Send(new ChangePasswordCommand(passwordDto.CurrentPassword, passwordDto.NewPassword, passwordDto.ConfirmedNewPassword, email));
            return Ok(result);
        }
        [HttpPut("UpdateProfile"), Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command, CancellationToken ct)
        {
            var result = await _imediator.Send(command, ct);

            if (result is null)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
