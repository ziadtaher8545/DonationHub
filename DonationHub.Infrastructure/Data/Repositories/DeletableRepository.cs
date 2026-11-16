using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Repositories
{
    public class DeletableRepository<TEntity> : IDeletableRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DonationDBContext _context;

        public DeletableRepository(DonationDBContext context)
        {
            _context = context;
        }

        public void Delete(TEntity entity, CancellationToken ct = default)
        {
            _context.Set<TEntity>().Remove(entity);

        }
    }
}
