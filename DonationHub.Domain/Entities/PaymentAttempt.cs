using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class PaymentAttempt : BaseEntity
    {
        public decimal Amount { get; private set; }
        public PaymentAttemptStatus MyProperty { get; private set; }
        public string? FailureCode { get; private set; }


        public string DonationId { get; private set; }
        public Donation Donation { get; private set; }


        // Payment
        public string? PaymenStripePaymentIntentId { get; private set; }
        public string StripeCheckoutSessionId { get; private set; }
        public static PaymentAttempt Create(decimal amount, PaymentAttemptStatus status, string? failureCode, string donationId, string? paymenStripePaymentIntentId, string stripeCheckoutSessionId)
        {
            return new PaymentAttempt
            {
                Amount = amount,
                MyProperty = status,
                FailureCode = failureCode,
                DonationId = donationId,
                PaymenStripePaymentIntentId = paymenStripePaymentIntentId,
                StripeCheckoutSessionId = stripeCheckoutSessionId
            };
        }














    }
}
