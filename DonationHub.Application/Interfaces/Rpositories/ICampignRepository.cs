using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.Rpositories
{
    public interface ICampaignRepository : IGenericRepository<Campaign>, IDeletableRepository<Campaign>
    {
        public Result MakeCheckRowVersion(Campaign campaign, string rowVersion);


    }
}
