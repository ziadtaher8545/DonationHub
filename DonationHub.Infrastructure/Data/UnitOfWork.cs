using DonationHub.Application.Interfaces;
using DonationHub.Infrastructure.Data.AppDBContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DonationDBContext _context;

        public UnitOfWork(DonationDBContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
