using DonationHub.Domain.Entities;

namespace DonationHub.Application.Interfaces
{
    public interface ITokenRepository : IGenericRepsitory<RefreshToken>
    {
        public Task<RefreshToken> GetByTokenHashAsync(string token, CancellationToken ct);
    }
}
