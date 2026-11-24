using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DonationHub.Application.Specifications.Category
{
    public class CampaignSpecification : BaseSpecification<DonationHub.Domain.Entities.Campaign>
    {
        public CampaignSpecification(Expression<Func<Domain.Entities.Campaign, bool>> criteria=null) : base(criteria)
        {
            AddInclude(c => c.Category);

        }

    }
}
