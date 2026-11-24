using DonationHub.Application.Specifications;
using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Specifications
{
    internal static class SpecificationEvaluator
    {
        internal static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> baseQuery, ISpecification<TEntity> spec) where TEntity : BaseEntity
        {
            var curQuery = baseQuery;

            if (spec.Criteria != null)
            {
                curQuery = curQuery.Where(spec.Criteria);
            }
            if (spec.IncludeExpresions.Any())
            {
                curQuery = spec.IncludeExpresions.Aggregate(curQuery, (cur, nextExp) => cur.Include(nextExp));

            }
            if (spec.OrderBy != null)
                curQuery = curQuery.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc != null)
                curQuery = curQuery.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPagination)
                curQuery = curQuery.Skip(spec.Skip).Take(spec.Take);
            return curQuery;

        }
    }
}
