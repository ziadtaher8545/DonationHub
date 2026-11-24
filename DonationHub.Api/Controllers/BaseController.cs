using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DonationHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator imediator)
        {
            _mediator = imediator;
        }



        protected string GetEmailByToken() => User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("You must to Log in First.");

        protected ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(new { isSucess = true, Data = result.Data });
            else
            {
                return ToProblem(result.Error);
            }

        }
        protected ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(new { isSuccess = true });

            return ToProblem(result.Error);
        }
        protected static ObjectResult ToProblem(Error error)
        {


            var status = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.PayloadTooLarge => StatusCodes.Status413PayloadTooLarge,
                ErrorType.UnprocessableEntity => StatusCodes.Status422UnprocessableEntity,
                ErrorType.TooManyRequests => StatusCodes.Status429TooManyRequests,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
            var problem = new ProblemDetails
            {
                Status = status,
                Title = error.Message,
                Type = $"https://httpstatuses.io/{status}",
                Extensions = {
                    ["code"] = error.Code
                }
            };

            return new ObjectResult(problem)
            {
                StatusCode = status
            };
        }


    }
}
