using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "EGP";
        private Money(decimal amount, string currency = "EGP")
        {

            Amount = amount;
            Currency = currency;
        }
        public static Result<Money> Create(decimal amount, string currency = "EGP")
        {
            //if (amount <= 0)
            //    return MoneyErrors.InvalidAmount;

            //if (string.IsNullOrWhiteSpace(currency))
            //    return MoneyErrors.InvalidCurrency;
            return new Money(amount, currency);
        }

        protected override IEnumerable<object?> GetValues()
        {
            yield return Amount;
            yield return Currency;
        }

    }
}
