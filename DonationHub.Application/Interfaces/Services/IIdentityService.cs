using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Admin.CreateStaff;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DonationHub.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> CreateUserAsync(RegisterCommand register, CancellationToken ct = default);
        public Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default);


        public Task<Result> CheckEmailExistAsync(string email, CancellationToken ct = default);

        public Task<Result> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default);

        public Task<Result<UserDto>> LoginAsync(LoginCommand login, CancellationToken ct = default);
        public Task<Result<UserDataDto>> GetUserByEmailAsync(string email, CancellationToken ct = default);
        public Task<Result<UserDataDto>> GetUserByIdAsync(Guid id, CancellationToken ct = default);
        //public Task<Result<string>> GetUserFullNameByIdAsync(Guid Id, CancellationToken ct = default);
        public Task<Result<IReadOnlyList<UserInfoDto>>> GetUsersByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken ct = default);


        public Task<Result> CreateRoleForUser(string email, string role, CancellationToken ct = default);
        public Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword, CancellationToken ct = default);

        public Task<Result<UserDataDto>> UpdateProfileAsync(string email, string fullName, string preferredLanguage, CancellationToken ct = default);
        public Task<Result<UserDataDto>> CreateStaffAsync(CreateStaffCommand command, CancellationToken ct = default);
        //public Task<Result> LogoutAsync(stri);


    }
}
