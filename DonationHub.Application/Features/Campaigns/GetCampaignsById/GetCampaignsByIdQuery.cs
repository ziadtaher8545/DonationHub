using DonationHub.Application.Dtos;
using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Campaigns.GetCampaignsById
{
    public record GetCampaignsByIdQuery(Guid Id) : IRequest<Result<CampaignDto>>
    {
        private string language;



        public string Language
        {
            get => language;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.ToLower() != "en"! || value.ToLower() != "ar"!)
                    language = "en";
                else language = value;


            }
        }
    }
}
