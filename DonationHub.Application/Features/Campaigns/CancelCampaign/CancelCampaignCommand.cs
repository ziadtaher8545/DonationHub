using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.CancelCampaign
{
    public record CancelCampaignCommand(Guid Id, string Email) : IRequest<Result>
    {
    }
}
