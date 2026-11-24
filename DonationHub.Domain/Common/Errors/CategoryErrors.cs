using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error NotFound = new("Category.NotFound", "Category was not found.", ErrorType.NotFound);

        public static readonly Error HasCampaigns = new("Category.HasCampaigns", "Cannot delete a category that has linked campaigns.", ErrorType.Conflict);
    }
}
