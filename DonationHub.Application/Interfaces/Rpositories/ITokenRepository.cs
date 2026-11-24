using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;

namespace DonationHub.Application.Interfaces.Rpositories
{
    public interface ITokenRepository : IGenericRepository<RefreshToken>
    {

        public Task<Result<RefreshToken>> GetByTokenHashAsync(string token, CancellationToken ct);
        Task<Result> RevokeFamilyAsync(Guid familyId, CancellationToken ct);
    }
}
