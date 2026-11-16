using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; protected set; }
        public DateTimeOffset? DeletedAt { get; protected set; }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }


    }
}
