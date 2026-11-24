using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public record CampaignDto
        (Guid Id, string Title, string Description, Money GoalAmount, Money RaiseAmount, DateTimeOffset EndDate, CampaignStatus Status, string? BannerFilName, string CategoryName, Guid CategoryId, string CreatedByFullName, Guid UserId, string RowVersion, int ProgressPrecent, int DaysLeft)
    {



    }
}
