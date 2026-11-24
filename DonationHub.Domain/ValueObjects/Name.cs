using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public class Name : ValueObject
    {

        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;
        public Name(string nameAr, string nameEn)
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }
        protected override IEnumerable<object> GetValues()
        {
            yield return NameAr;
            yield return NameEn;

        }
    }
}
