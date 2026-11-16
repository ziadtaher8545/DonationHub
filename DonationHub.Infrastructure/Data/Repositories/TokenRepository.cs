using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.AppDBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Repositories
{
    internal class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        public TokenRepository(DonationDBContext context) : base(context)
        {
        }

        public async Task<RefreshToken> GetByTokenHashAsync(string token, CancellationToken ct)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rf => rf.TokenHash == token && rf.ExpiredAt > DateTimeOffset.UtcNow, ct);

            return refreshToken;
        }
    }
}
