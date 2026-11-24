using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
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


        public static Result<Donation> Create(Money amount, string? failureReason, string? receiptNumber, string? receiptFileName, Guid campaignId, Guid donorUserId, decimal maxAmount, decimal minAmount, DateTimeOffset endDate)
        {
            if (amount.Amount < minAmount || amount.Amount > maxAmount)
            {
                return DonationErrors.AmountOutOfRange;
            }
            if (endDate <= DateTimeOffset.UtcNow)
                return DonationErrors.InvalidState;
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
