using DonationHub.Domain.Enums;
using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class Donation : BaseEntity
    {
        public Money Amount { get; private set; }
        public DonationStatus Status { get; private set; }
        public string? FailureReason { get; private set; }
        public string? ReceiptNumber { get; private set; }
        public string? ReceiptFileName { get; private set; }


        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }


        public Guid DonorUserId { get; private set; }
        public DateTimeOffset EndDate { get; private set; }


        public static Donation Create(Money amount, string? failureReason, string? receiptNumber, string? receiptFileName, Guid campaignId, Guid donorUserId, decimal maxAmount, decimal minAmount, DateTimeOffset endDate)
        {
            if (amount.Amount < minAmount || amount.Amount > maxAmount)
            {
                throw new ArgumentException($"Donation amount must be between {minAmount} and {maxAmount}.", nameof(amount));
            }
            if (endDate <= DateTimeOffset.UtcNow)
                throw new ArgumentException("End date must be in the future.", nameof(endDate));

            return new Donation
            {

                Amount = amount,
                Status = DonationStatus.Pending,
                FailureReason = failureReason,
                ReceiptNumber = receiptNumber,
                ReceiptFileName = receiptFileName,
                CampaignId = campaignId,
                DonorUserId = donorUserId
                ,
                EndDate = endDate
            };
        }




    }
}
