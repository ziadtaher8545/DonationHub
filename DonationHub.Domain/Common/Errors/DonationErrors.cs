using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class DonationErrors
    {
        public static readonly Error NotFound = new("Donation.NotFound", "Donation was not found or does not belong to the current user.", ErrorType.NotFound);

        public static readonly Error AmountOutOfRange = new("Donation.AmountOutOfRange", "The donation amount is outside the allowed range.", ErrorType.UnprocessableEntity);

        public static readonly Error InvalidState = new("Donation.InvalidState", "The donation is already in a final state.", ErrorType.Conflict);


    }
}