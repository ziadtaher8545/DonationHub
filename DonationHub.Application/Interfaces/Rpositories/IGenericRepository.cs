using DonationHub.Application.Specifications;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<List<TEntity>>> GetAllAsync(CancellationToken ct = default);
        Task<Result> CreateAsync(TEntity entity, CancellationToken ct = default);
        Task<Result> UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task<Result<TEntity>> GetByIdAsync(Guid id, ISpecification<TEntity> spec, CancellationToken ct = default);
        Task<Result<List<TEntity>>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken ct = default);

    }
}
