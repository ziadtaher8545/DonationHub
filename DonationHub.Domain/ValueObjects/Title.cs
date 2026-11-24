using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public  class Title : ValueObject
    {
        public string TitleAr { get; set; } = default!;
        public string TitleEn { get; set; } = default!;
        public Title(string titleAr, string titleEn)
        {
            TitleAr = titleAr;
            TitleEn = titleEn;
        }
        protected override IEnumerable<object> GetValues()
        {
            yield return TitleAr;
            yield return TitleEn;
        }
    }
}
