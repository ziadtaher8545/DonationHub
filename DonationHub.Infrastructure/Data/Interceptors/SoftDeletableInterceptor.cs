
using DonationHub.Domain.Interfaces;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DonationHub.Infrastructure.Data.Interceptors;

internal class SoftDeletableInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public SoftDeletableInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }


    private void ApplySoftDelete(DbContext? context)
    {
        if (context is null) return;

        var now = _timeProvider.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>()
                                     .Where(e => e.State == EntityState.Deleted)
                                     .ToList())
        {
            entry.State = EntityState.Modified;
            entry.Entity.Delete(now);

            foreach (var reference in entry.References)
            {
                if (reference.TargetEntry?.Metadata.IsOwned() == true)
                    reference.TargetEntry.State = EntityState.Unchanged;
            }
        }
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
