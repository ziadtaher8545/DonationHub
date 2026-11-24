using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.IdentityModel.Tokens.Experimental;
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

        public Result Delete(TEntity entity, CancellationToken ct = default)
        {
           var result= _context.Set<TEntity>().Remove(entity);
            if (result == null)
                return ValidationErrors.Failed;
            return Result.Ok();


        }
    }
}
