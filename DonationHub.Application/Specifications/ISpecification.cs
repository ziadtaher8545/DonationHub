using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DonationHub.Application.Specifications
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        ICollection<Expression<Func<TEntity, object>>> IncludeExpresions { get; }
        Expression<Func<TEntity, bool>> Criteria { get; }

        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagination { get; }

    }
}
