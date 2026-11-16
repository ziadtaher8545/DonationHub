using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "EGP";
        public Money(decimal amount, string currency = "EGP")
        {
            if (Amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative.");
            }
            Amount = amount;
            Currency = currency;
        }


    }
}
