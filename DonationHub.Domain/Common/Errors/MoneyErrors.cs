using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class MoneyErrors
    {
        public static readonly Error InvalidAmount = new("Money.InvalidAmount", "Amount should be Greater than 0. ", ErrorType.Validation);
        public static readonly Error InvalidCurrency = new("Money.InvalidCurrency", "Currency shouldn't be Empty. ", ErrorType.Validation);

    }
}
