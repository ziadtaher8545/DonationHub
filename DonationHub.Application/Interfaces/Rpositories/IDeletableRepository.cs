using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.Rpositories;

public interface IDeletableRepository<TEntity> where TEntity : BaseEntity
{
    Result Delete(TEntity entity, CancellationToken ct = default);
}

