using DonationHub.Application.Features.Admin.CreateStaff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {

        public AdminController(IMediator mediator) : base(mediator)
        {

        }
        [HttpPost("staff")]
        public async Task<IActionResult> staff(CreateStaffCommand command)
        {
            var result = await _imediator.Send(command);
            if (!result)
                return ValidationProblem();

            return Ok(result);
        }
    }
}
