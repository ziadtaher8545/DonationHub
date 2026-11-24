using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.DeleteCampaign
{
    internal class DeleteCampaignHandler : IRequestHandler<DeleteCampaignCommand, Result>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCampaignHandler(ICampaignRepository campaignRepository, IUnitOfWork unitOfWork)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.GetByIdAsync(request.Id, cancellationToken);
            if (!campaign.IsSuccess) return campaign.Error!;
            var result = _campaignRepository.Delete(campaign.Data);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result;

        }
    }
}
