using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public class Description : ValueObject
    {
        public string DescriptionAr { get; set; } = default!;
        public string DescriptionEn { get; set; } = default!;
       public Description(string descriptionAr, string descriptionEn)
        {
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
        }
    }
}
