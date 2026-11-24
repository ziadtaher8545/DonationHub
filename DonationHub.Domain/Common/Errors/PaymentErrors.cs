using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{



    public static class PaymentErrors
    {
        public static readonly Error InvalidSignature = new("Payment.InvalidSignature", "The payment webhook signature is invalid.", ErrorType.Validation);
    }

}
