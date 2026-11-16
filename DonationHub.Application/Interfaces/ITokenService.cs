using DonationHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces
{
    public interface ITokenService
    {

        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles);
        public (string, RefreshToken) CreateRefreshToken(Guid userId);
        public string HashToken(string token);

    }
}
