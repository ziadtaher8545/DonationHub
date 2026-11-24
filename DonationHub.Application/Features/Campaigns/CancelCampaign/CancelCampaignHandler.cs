using DonationHub.Application.Features.Campaigns.CancelCampaign;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using MediatR;

internal class CancelCampaignHandler
    : IRequestHandler<CancelCampaignCommand, Result>
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly IIdentityService _identityService;

    public CancelCampaignHandler(
        ICampaignRepository campaignRepository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IIdentityService identityService)
    {
        _campaignRepository = campaignRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _identityService = identityService;
    }

    public async Task<Result> Handle(CancelCampaignCommand request, CancellationToken cancellationToken)
    {
        // Owner
        var userResult = await _identityService.GetUserByEmailAsync(request.Email, cancellationToken);

        if (!userResult.IsSuccess)
            return userResult.Error!;
        var user = userResult.Data;


        //Admin
        var isAdmin = user.Roles.Contains("Admin");




        var campaignResult = await _campaignRepository.GetByIdAsync(request.Id, cancellationToken);

        if (!campaignResult.IsSuccess)
            return campaignResult.Error!;

        var campaign = campaignResult.Data;
        var isOwner = campaign.CreatedByUserId == user.Id;
        if (!isAdmin && !isOwner)
            return AuthErrors.Unauthorized;

        var result = campaign.Cancel(_timeProvider.GetUtcNow());

        if (!result.IsSuccess)
            return result.Error!;

        var finalResult = await _campaignRepository.UpdateAsync(campaign, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return finalResult;
    }
}