using DonationHub.Application.Dtos;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Application.Specifications.Category;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.UpdateCampaign
{
    internal class UpdateCampaignHandler : IRequestHandler<UpdateCampaignCommand, Result<CampaignFullDto>>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;
        private readonly IIdentityService _identityService;



        public UpdateCampaignHandler(ICampaignRepository campaignRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider, IIdentityService identityService)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
            _identityService = identityService;
        }

        public async Task<Result<CampaignFullDto>> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
        {
            //var campaignResult = await _campaignRepository.GetByIdAsync(request.Id);// should add spec to include category Navigation property that cause Null Exception

            var spec = new CampaignSpecification();

            var campaignResult = await _campaignRepository.GetByIdAsync(request.Id, spec, cancellationToken);
            if (!campaignResult.IsSuccess)
                return CampaignErrors.NotFound;
            var campaign = campaignResult.Data;

            var moneyResult = Money.Create(request.GoalAmount, request.Currency);
            if (!moneyResult.IsSuccess) return moneyResult.Error!;


            var res = campaign.Update(new Title(request.TitleAr, request.TitleEn), new Description(request.DescriptionAr, request.DescriptionEn),
                 moneyResult.Data!, request.EndDate, _timeProvider.GetUtcNow());
            if (!res.IsSuccess)
                return res.Error!;

            var user = await _identityService.GetUserByIdAsync(campaign.CreatedByUserId, cancellationToken);
            if (!user.IsSuccess) return user.Error!;
            var result = _campaignRepository.MakeCheckRowVersion(campaign, request.RowVersion);
            if (!result.IsSuccess)
                return result.Error;

            var resultUpdate = await _campaignRepository.UpdateAsync(campaign);
            if (!resultUpdate.IsSuccess) return resultUpdate.Error!;

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return CampaignErrors.ConcurrencyConflict;
            }
            var rowVersion = Convert.ToBase64String(campaign.RowVersion);
            return new CampaignFullDto(campaign.Id, campaign.Title, campaign.Description, campaign.GoalAmount, campaign.RaiseAmount, campaign.EndDate, campaign.Status, campaign.BannerFileName, campaign.Category.Name, campaign.CategoryId, user.Data.FullName, campaign.CreatedByUserId, rowVersion, campaign.ProgressPrecent, campaign.DaysLeft);

        }
    }
}
