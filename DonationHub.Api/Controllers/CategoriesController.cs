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
    public class CategoriesController : BaseController
    {
        public CategoriesController(IMediator imediator) : base(imediator)
        {
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken ct)
        {
            var categories = await _imediator.Send(new GetAllCategoriesQuery(), ct);
            return Ok(categories);
        }
        [HttpGet("{id:guid}")]

        public async Task<IActionResult> GetCategory(Guid id, CancellationToken ct)
        {
            var category = await _imediator.Send(new GetCategoryByIdQuery(id), ct);
            if (category is null) return NotFound(); 
            return Ok(category);
        }




        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand command, CancellationToken ct)
        {
            var categoryId = await _imediator.Send(command, ct);
            return Ok(categoryId);

        }
        [Authorize(Roles = "Admin"), HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryToUpdateDto request, CancellationToken ct)
        {

            var command = new UpdateCategoryCommand(id, request.Name, request.Description);
            var result = await _imediator.Send(command, ct);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [Authorize(Roles = "Admin"), HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            var resultDelete = await _imediator.Send(new DeleteCategoryCommand(id), ct);
            // must be edited ___
            if (!resultDelete)
                return NotFound();
            return Ok(resultDelete);
        }



    }
}
