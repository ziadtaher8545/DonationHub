using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.GetAllSuccessCampaigns
{
    internal class GetAllSuccessCampaignsHandler : IRequestHandler<GetAllSuccessCampaignsQuery, Result<List<CampaignDto>>>
    {
        private readonly IDonationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetAllSuccessCampaignsHandler(IDonationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result<List<CampaignDto>>> Handle(GetAllSuccessCampaignsQuery request, CancellationToken cancellationToken)
        {
            var campaigns = await _context.Campaigns.Where(ca => ca.Status == CampaignStatus.Completed).AsNoTracking().Include(c => c.Category).ToListAsync(cancellationToken);

            var userIds = campaigns.Select(c => c.CreatedByUserId).Distinct().ToArray();

            var usersResult = await _identityService.GetUsersByIdsAsync(userIds);

            if (!usersResult.IsSuccess)
                return CampaignErrors.NotFound;

            var usersDictionary = usersResult.Data.ToDictionary(u => u.Id);

            var finalCampaigns = campaigns.Select(c =>
            {
                usersDictionary.TryGetValue(c.CreatedByUserId, out var user);
                var rowVersion = Convert.ToBase64String(c.RowVersion);
                var title = request.Language == "ar" ? c.Title.TitleAr : c.Title.TitleEn;
                var description = request.Language == "ar" ? c.Description.DescriptionAr : c.Description.DescriptionEn;
                var categoryName = request.Language == "ar" ? c.Category.Name.NameAr : c.Category.Name.NameEn;

                return new CampaignDto(c.Id, title, description, c.GoalAmount, c.RaiseAmount, c.EndDate, c.Status, c.BannerFileName, categoryName, c.CategoryId, user?.FullName ?? "Unknown", c.CreatedByUserId, rowVersion, c.ProgressPrecent, c.DaysLeft);
            }).ToList();

            return finalCampaigns;
        }
    }
}
