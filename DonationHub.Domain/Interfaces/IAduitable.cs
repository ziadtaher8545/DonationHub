using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Interfaces
{
    public interface IAduitable
    {
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? UpdatedAt { get; }

    }
}
