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
        public async Task<ActionResult> staff(CreateStaffCommand command)
        {
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
