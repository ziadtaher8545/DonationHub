using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DonationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _imediator;

        public BaseController(IMediator imediator)
        {
            _imediator = imediator;
        }



        protected string GetEmailByToken() => User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("You must to Log in First.");
        protected string GetIdByToken() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("You must to Log in First.");
    }
}
