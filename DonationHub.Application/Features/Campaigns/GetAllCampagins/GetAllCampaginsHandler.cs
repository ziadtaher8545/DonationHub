using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.GetAllCampagins
{
    internal class GetAllCampaginsHandler : IRequestHandler<GetAllCampaginsQuery, Result<List<CampaignDto>>>
    {
        private readonly IDonationDbContext _context;
        private readonly IIdentityService _identityService;

        public GetAllCampaginsHandler(IDonationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result<List<CampaignDto>>> Handle(GetAllCampaginsQuery request, CancellationToken cancellationToken)
        {
            //var campagins = await _context.Campaigns.AsNoTracking().Include(ca => ca.Category).ToListAsync(cancellationToken);
            ////var usersFullnames = campagins.Select(async ca => new
            ////{
            ////    FullName = (await _identityService.GetUserByIdAsync(ca.CreatedByUserId)).Data.FullName
            ////});
            //List<CampaginDto> finalCampagins = [];
            //foreach (var campagin in campagins)
            //{
            //    var userFullName = await _identityService.GetUserByIdAsync(request.Id);
            //    if (!userFullName.IsSuccess)
            //        return CampaignErrors.NotFound;
            //    finalCampagins.Add(new CampaginDto(campagin.Id, campagin.Title, campagin.Description, campagin.GoalAmount, campagin.EndDate, campagin.BannerFileName, campagin.Category.Name, campagin.CategoryId, userFullName.Data.FullName, campagin.CreatedByUserId));
            //}


            var campaigns = await _context.Campaigns.Where(ca => ca.Status == CampaignStatus.Active).AsNoTracking().Include(c => c.Category).ToListAsync(cancellationToken);

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
