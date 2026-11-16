using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Entities
{
    public class ProcessedStripeEvent : BaseEntity
    {
        /*
          Configurations needs 
        1- ignore  UpdateAt
        2- Rename CreatedAt => ProcessAt
         */

        public string Type { get; private set; } = default!;
        public static ProcessedStripeEvent Create(string type)
        {
            return new ProcessedStripeEvent
            {
                Type = type
            };
        }
    }
}
