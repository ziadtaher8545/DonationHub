using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DonationHub.Application.Specifications
{
    public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {


        public ICollection<Expression<Func<TEntity, object>>> IncludeExpresions { get; } = [];

        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }
        public bool IsPagination { get; private set; }

        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            IncludeExpresions.Add(include);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;

        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        protected void AddPagination(int pageSize, int pageNumber)
        {
            IsPagination = true;
            Take = pageSize;
            Skip = (pageNumber - 1) * pageSize;
        }
    }
}
