using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public record CampaignToCreateDto(string TitleEn, string TitleAr, string? DescriptionAr, string? DescriptionEn, decimal GoalAmount,
    string Currency, string? BnnerFileName, DateTimeOffset EndDate, Guid CategoryId)
    {
    }
}
