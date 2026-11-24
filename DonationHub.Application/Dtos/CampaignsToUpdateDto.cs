using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public record CampaignsToUpdateDto(string TitleEn, string TitleAr, string DescriptionEn, string DescriptionAr, decimal GoalAmount, string Currency, DateTimeOffset EndDate, string? BannerFileName, string RowVersion)

    {
    }
}
