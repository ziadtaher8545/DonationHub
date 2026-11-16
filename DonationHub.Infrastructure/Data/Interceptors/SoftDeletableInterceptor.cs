using DonationHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Interceptors
{
    internal class SoftDeletableInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {

            var context = eventData.Context;
            if (context is null)
                return base.SavingChanges(eventData, result);
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry is null || entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity })
                    continue;
                entry.State = EntityState.Modified;
                entity.Delete();
            }
            return base.SavingChanges(eventData, result);

        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry is null || entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity })
                    continue;
                entry.State = EntityState.Modified;
                entity.Delete();
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


    }
}
