using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Categories.CreateCategory;
using DonationHub.Application.Features.Categories.DeleteCategory;
using DonationHub.Application.Features.Categories.GetAllCategories;
using DonationHub.Application.Features.Categories.GetCategoryById;
using DonationHub.Application.Features.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IMediator imediator) : base(imediator)
        {
        }
        [HttpGet, AllowAnonymous]

        public async Task<ActionResult<List<CategoryDto>>> GetCategories(CancellationToken ct)
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery(), ct);
            return ToActionResult(categories);
        }
        [HttpGet("{id:guid}"), AllowAnonymous]

        public async Task<ActionResult<CategoryDto>> GetCategory([FromRoute]Guid id, CancellationToken ct)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id), ct);
            return ToActionResult(category);
        }




        [HttpPost]
        public async Task<ActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return ToActionResult(result);

        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryToUpdateDto>> UpdateCategory(Guid id, [FromBody] CategoryToUpdateDto request, CancellationToken ct)
        {

            var command = new UpdateCategoryCommand(id, request.Name, request.Description);
            var result = await _mediator.Send(command, ct);
            return ToActionResult(result);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            var resultDelete = await _mediator.Send(new DeleteCategoryCommand(id), ct);
            return ToActionResult(resultDelete);
        }



    }
}
