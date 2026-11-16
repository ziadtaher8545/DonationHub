using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepsitory<TEntity> where TEntity : notnull,
        BaseEntity
    {
        protected readonly DonationDBContext _context;

        public GenericRepository(DonationDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            await _context.AddAsync(entity, ct);
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
        => await _context.Set<TEntity>().ToListAsync(ct);

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
