using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.Register;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DonationHub.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> CreateUserAsync(RegisterCommand register, CancellationToken ct = default);
        public Task<IReadOnlyList<string>> GetRolesAsync(string email, CancellationToken ct = default);


        public Task<bool> CheckEmailExistAsync(string email, CancellationToken ct = default);

        public Task<bool> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

        public Task<UserDto> LoginAsync(LoginCommand login, CancellationToken ct = default);
        public Task<UserDataDto> GetUserByEmailAsync(string email, CancellationToken ct = default);
        public Task<IdentityUserDto?> GetUserByIdAsync(Guid Id, CancellationToken ct = default);


        public Task<bool> CreateRoleForUser(string email, string role, CancellationToken ct = default);
        public Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword, CancellationToken ct = default);

        public Task<UserDataDto> UpdateProfileAsync(string email, string fullName, string preferredLanguage, CancellationToken ct = default);


    }
}
