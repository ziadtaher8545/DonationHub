using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Campaigns.CancelCampaign;
using DonationHub.Application.Features.Campaigns.CreateCampaign;
using DonationHub.Application.Features.Campaigns.DeleteCampaign;
using DonationHub.Application.Features.Campaigns.GetAllCampagins;
using DonationHub.Application.Features.Campaigns.GetAllSuccessCampaigns;
using DonationHub.Application.Features.Campaigns.GetCampaignsById;
using DonationHub.Application.Features.Campaigns.UpdateCampaign;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.Api.Controllers
{
    public class CampaignsController : BaseController
    {

        public CampaignsController(IMediator mediator) : base(mediator)
        {

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> CreateCampagin(CampaignToCreateDto request, CancellationToken ct)
        {
            var email = GetEmailByToken();
            return ToActionResult(await _mediator.Send(new CreateCampaignCommand(request.TitleEn, request.TitleAr, request.DescriptionAr, request.DescriptionEn, request.GoalAmount, request.Currency, request.BnnerFileName, request.EndDate, request.CategoryId, email)));

        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<List<CampaignDto>>> GetCampaigns(CancellationToken ct)
        {

            var query = new GetAllCampaginsQuery();
            query.Language = Request.Headers.AcceptLanguage.ToString();

            var campagins = await _mediator.Send(query, ct);
            return ToActionResult(campagins);
        }
        [HttpGet("success-stories"), AllowAnonymous]
        public async Task<ActionResult<List<CampaignDto>>> GetSucessCampaigns(CancellationToken ct)
        {


            var query = new GetAllSuccessCampaignsQuery() { Language = Request.Headers.AcceptLanguage.ToString() };
            var campagins = await _mediator.Send(query, ct);
            return ToActionResult(campagins);
        }
        [HttpGet("{id:guid}"), AllowAnonymous]
        public async Task<ActionResult<CampaignDto>> GetCampaignById([FromRoute] Guid id, CancellationToken ct)
        {
            var request = new GetCampaignsByIdQuery(id);
            request.Language = Request.Headers.AcceptLanguage.ToString();
            return ToActionResult(await _mediator.Send(request));
        }
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> DeleteCampaigns([FromRoute] DeleteCampaignCommand request, CancellationToken ct)
        {
            return ToActionResult(await _mediator.Send(request));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<CampaignFullDto>> UpdateCampaign([FromRoute] Guid id, [FromBody] CampaignsToUpdateDto request, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateCampaignCommand(id, request.TitleEn, request.TitleAr, request.DescriptionEn, request.DescriptionAr, request.GoalAmount, request.Currency, request.EndDate, request.BannerFileName, request.RowVersion));
            return ToActionResult(result);


        }

        [HttpPatch("{id:guid}/cancel")]
        [Authorize]
        public async Task<ActionResult> CancelCampaign([FromRoute] Guid id, CancellationToken ct)
        {
            var userEmail = GetEmailByToken();
            var result = await _mediator.Send(new CancelCampaignCommand(id, userEmail), ct);

            return ToActionResult(result);
        }

    }
}
