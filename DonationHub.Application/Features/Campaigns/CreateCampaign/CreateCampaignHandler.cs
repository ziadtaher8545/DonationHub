using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Domain.ValueObjects;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign
{
    internal class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, Result>
    {
        private readonly ICampaignRepository _campignRepository;
        private readonly IIdentityService _identityService;
        private readonly TimeProvider _timeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCampaignHandler(ICampaignRepository campignRepository, TimeProvider timeProvider, IIdentityService identityService, IUnitOfWork unitOfWork)
        {
            _campignRepository = campignRepository;
            _timeProvider = timeProvider;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.GetUserByEmailAsync(request.Email);
            if (!user.IsSuccess)
                return user.Error!;

            var moneyResult = Money.Create(request.GoalAmount, request.Currency);
            if (!moneyResult.IsSuccess) return moneyResult.Error!;
            var campignResult = Campaign.Create(new Title(request.TitleAr, request.TitleEn), new Description(request.DescriptionAr, request.DescriptionEn), moneyResult.Data, request.EndDate, request.BnnerFileName, request.CategoryId, user.Data.Id, _timeProvider.GetUtcNow());
            if (!campignResult.IsSuccess)
                return campignResult.Error!;
            var createResult = await _campignRepository.CreateAsync(campignResult.Data, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return createResult;
        }
    }
}
