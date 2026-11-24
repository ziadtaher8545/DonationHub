using DonationHub.Application.Dtos;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.Services
{
    public interface ITokenService
    {

        public Result<String> CreateToken(string userId, string email, string userName, IEnumerable<string> roles);
        public Result<(string, RefreshToken)> CreateRefreshToken(Guid userId, Guid familyId);
        public Result<string> HashToken(string token);
        public Task<Result<string>> ActivateRefreshToken(string oldRefreshToken, CancellationToken ct = default);

    }
}
