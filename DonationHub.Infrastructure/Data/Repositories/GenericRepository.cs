using DonationHub.Application.Interfaces;
using DonationHub.Application.Specifications;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using DonationHub.Infrastructure.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace DonationHub.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity

    {
        protected readonly DonationDBContext _context;

        public GenericRepository(DonationDBContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(TEntity entity, CancellationToken ct = default)
        {
            var result = await _context.AddAsync(entity, ct);
            if (result == null) return ValidationErrors.Failed;
            return Result.Ok();



        }

       

        public async Task<Result<List<TEntity>>> GetAllAsync(CancellationToken ct = default)
        => await _context.Set<TEntity>().AsNoTracking().ToListAsync(ct);

        public async Task<Result<List<TEntity>>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        {
            var baseQuery = _context.Set<TEntity>().AsNoTracking();
            var fullQuery = SpecificationEvaluator.CreateQuery(baseQuery, spec);

            return await fullQuery.ToListAsync(ct);
        }

        public async Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id, ct);
            if (entity == null)
                return ValidationErrors.Failed;
            return entity;
        }

        public async Task<Result<TEntity>> GetByIdAsync(Guid id, ISpecification<TEntity> spec, CancellationToken ct = default)
        {
            var baseQuery = _context.Set<TEntity>();
            var fullQuery = SpecificationEvaluator.CreateQuery(baseQuery, spec);

            var entity = await fullQuery.FirstOrDefaultAsync(ct);

            if (entity == null)
                return ValidationErrors.Failed;
            return entity;
        }

        public async Task<Result> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            var result = _context.Set<TEntity>().Update(entity);
            if (result == null)
                return ValidationErrors.Failed;
            return Result.Ok();
        }

   
    }
}
