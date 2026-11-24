using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; protected set; }
        public DateTimeOffset? DeletedAt { get; protected set; }

        public void Delete(DateTimeOffset now)
        {
            IsDeleted = true;
            DeletedAt = now;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }


    }
}
