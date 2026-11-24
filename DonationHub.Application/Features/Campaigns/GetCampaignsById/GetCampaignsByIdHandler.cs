using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.GetCampaignsById
{
    internal class GetCampaignsByIdHandler : IRequestHandler<GetCampaignsByIdQuery, Result<CampaignDto>>
    {
        private readonly IDonationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetCampaignsByIdHandler(IDonationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result<CampaignDto>> Handle(GetCampaignsByIdQuery request, CancellationToken cancellationToken)
        {
            var campaign = _context.Campaigns.Include(ca => ca.Category).AsNoTracking().ToList().FirstOrDefault(ca => ca.Id == request.Id);
            if (campaign == null)
                return CampaignErrors.NotFound;
            var user = await _identityService.GetUserByIdAsync(campaign.CreatedByUserId, cancellationToken);
            if (!user.IsSuccess) return user.Error;
            var rowVersion = Convert.ToBase64String(campaign.RowVersion);
            var title = request.Language == "ar" ? campaign.Title.TitleAr : campaign.Title.TitleEn;
            var description = request.Language == "ar" ? campaign.Description.DescriptionAr : campaign.Description.DescriptionEn;
            var categoryName = request.Language == "ar" ? campaign.Category.Name.NameAr : campaign.Category.Name.NameEn;

            return new CampaignDto(campaign.Id, title, description, campaign.GoalAmount, campaign.RaiseAmount, campaign.EndDate, campaign.Status, campaign.BannerFileName, categoryName, campaign.Category.Id, user.Data.FullName, campaign.CreatedByUserId, rowVersion, campaign.ProgressPrecent, campaign.DaysLeft);


        }
    }
}
