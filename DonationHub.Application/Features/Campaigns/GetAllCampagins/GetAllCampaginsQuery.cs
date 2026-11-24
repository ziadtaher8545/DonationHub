using DonationHub.Application.Dtos;
using DonationHub.Domain.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.GetAllCampagins
{
    public record GetAllCampaginsQuery : IRequest<Result<List<CampaignDto>>>
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
