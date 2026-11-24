using DonationHub.Domain.Common.Results;
using DonationHub.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign
{
    public record CreateCampaignCommand : IRequest<Result>
    {
        public CreateCampaignCommand(string titleEn, string titleAr, string? descriptionAr, string? descriptionEn, decimal goalAmount,
    string currency, string? bnnerFileName, DateTimeOffset endDate, Guid categoryId, string email)
        {
            TitleEn = titleEn;
            TitleAr = titleAr;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            GoalAmount = goalAmount;
            Currency = currency;
            BnnerFileName = bnnerFileName;
            EndDate = endDate;
            CategoryId = categoryId;
            Email = email;
        }

        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal GoalAmount { get; set; }
        public string Currency { get; set; }
        public string? BnnerFileName { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Email { get; set; }
    }
}
