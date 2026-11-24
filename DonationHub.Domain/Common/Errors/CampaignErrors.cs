using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors
{
    public static class CampaignErrors
    {
        public static readonly Error NotFound = new("Campaign.NotFound", "Campaign was not found.", ErrorType.NotFound);

        public static readonly Error NotActive = new("Campaign.NotActive", "The campaign is not active.", ErrorType.Conflict);

        public static readonly Error GoalBelowRaised = new("Campaign.GoalBelowRaised", "The campaign goal cannot be lower than the raised amount.", ErrorType.UnprocessableEntity);

        public static readonly Error EndDateInPast = new("Campaign.EndDateInPast", "The campaign end date must be in the future.", ErrorType.UnprocessableEntity);


        public static readonly Error ConcurrencyConflict = new("Campaign.ConcurrencyConflict", "The campaign was modified by another request. Please reload and try again.", ErrorType.Conflict);

        public static readonly Error InvalidGoalAmount = new("Campaign.InvalidGoalAmount", "The campaign goal amount must be greater than zero.", ErrorType.UnprocessableEntity);

        public static readonly Error InvalidRaiseAmount = new("Campaign.InvalidRaiseAmount", "The raised amount must be between zero and the campaign goal amount.", ErrorType.UnprocessableEntity);
        public static readonly Error CannotCancelCompleted = new("Campaign.CannotCancelCompleted", "A completed campaign cannot be cancelled.", ErrorType.Conflict);

        public static readonly Error CannotCancelExpired = new("Campaign.CannotCancelExpired", "An expired campaign cannot be cancelled.", ErrorType.Conflict);

        public static readonly Error AlreadyCancelled = new("Campaign.AlreadyCancelled", "The campaign has already been cancelled.", ErrorType.Conflict);

    }
}
