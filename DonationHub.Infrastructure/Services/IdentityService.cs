using DonationHub.Application.Common.Options;
using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Admin.CreateStaff;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Interfaces.Rpositories;
using DonationHub.Application.Interfaces.Services;
using DonationHub.Domain.Common.Errors;
using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DonationHub.Infrastructure.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _options;
        private readonly ITokenRepository _tokenRepo;


        public IdentityService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ITokenService tokenService, IOptions<JwtOptions> options, ITokenRepository tokenRepo)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _tokenRepo = tokenRepo;
        }

        public async Task<Result> CheckEmailExistAsync(string email, CancellationToken ct = default)
        {
            var result = await _userManager.FindByEmailAsync(email) is not null;
            if (!result) return AuthErrors.InvalidCredentials;

            return Result.Ok();
        }

        public async Task<Result> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return AuthErrors.InvalidCredentials;

            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {

                return AuthErrors.InvalidCredentials;


            }
            return Result.Ok();
        }

        public async Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return AuthErrors.InvalidCredentials;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }
        // should return result 
        public async Task<Result> CreateUserAsync(RegisterCommand register, CancellationToken ct = default)
        {
            var userExistResult = await GetUserByEmailAsync(register.Email);
            if (userExistResult.IsSuccess) return AuthErrors.EmailAlreadyExists;

            var user = ApplicationUser.Create(register.FullName, register.Email, register.UserName, register.PreferredLanguage);

            var resultUser = await _userManager.CreateAsync(user, register.Password);
            if (!resultUser.Succeeded)
            {
                return AuthErrors.InvalidCredentials;
            }

            var resultRole = await _userManager.AddToRoleAsync(user, "Donor");
            if (!resultRole.Succeeded)
                return AuthErrors.InvalidCredentials;


            //await _unitOfWork.SaveChangesAsync(); ملهاش لازمة علشان الميثود بتكريت وتسيف اليوزر لوحدها


            return Result.Ok();
        }

        public async Task<Result<UserDto>> LoginAsync(LoginCommand login, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user is null)
                return AuthErrors.InvalidCredentials;
            if (!user.IsActive)
                return AuthErrors.AccountDeactivated;



            var passwordCheck = await _userManager.CheckPasswordAsync(
                user,
                login.Password);

            if (!passwordCheck)
                return AuthErrors.InvalidCredentials;


            var roles = await _userManager.GetRolesAsync(user);

            var accessTokenResult = _tokenService.CreateToken(
                user.Id.ToString(),
                user.Email!,
                user.UserName!,
                roles.ToList());
            if (!accessTokenResult.IsSuccess) return accessTokenResult.Error!;
            var accessToken = accessTokenResult.Data;

            // Create a new token family for this login session
            var familyId = Guid.NewGuid();

            var refreshTokenResult =
            _tokenService.CreateRefreshToken(user.Id, familyId);
            if (!refreshTokenResult.IsSuccess)
                return refreshTokenResult.Error!;
            var (plainText, refreshToken) = refreshTokenResult.Data;

            await _tokenRepo.CreateAsync(refreshToken, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName!,
                AcessToken = accessToken,
                RefershToken = plainText
            };
        }

        public async Task<Result<UserDataDto>> GetUserByEmailAsync(string email, CancellationToken ct = default)
        {
            if (email == null)
                return AuthErrors.InvalidCredentials;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return AuthErrors.InvalidCredentials;

            var rolesResult = await GetRolesAsync(user.Email, ct);
            if (!rolesResult.IsSuccess)
                return AuthErrors.InvalidCredentials;
            return new UserDataDto(user.Id, user.Email, user.UserName, user.FullName, rolesResult.Data);

        }

        public async Task<Result> CreateRoleForUser(string email, string role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return AuthErrors.InvalidCredentials;


            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) return AuthErrors.InvalidCredentials;

            return Result.Ok();
        }

        public async Task<Result<UserDataDto>> GetUserByIdAsync(Guid Id, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user is null) return AuthErrors.InvalidCredentials;

            var rolesResult = await GetRolesAsync(user.Email, ct);
            if (!rolesResult.IsSuccess)
                return AuthErrors.InvalidCredentials;

            return new UserDataDto(user.Id, user.Email, user.UserName, user.FullName, rolesResult.Data, user.PreferredLanguage);



        }

        public async Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return AuthErrors.InvalidCredentials;


            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
                return Result.Ok();
            else return AuthErrors.InvalidCredentials;

        }

        public async Task<Result<UserDataDto>> UpdateProfileAsync(string email, string fullName, string preferredLanguage, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return AuthErrors.InvalidCredentials;

            user.ChangeFullName(fullName);
            user.ChangePreferredLanguage(preferredLanguage);
            var rolesResult = await GetRolesAsync(email);
            if (!rolesResult.IsSuccess)
                return AuthErrors.InvalidCredentials;


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new UserDataDto(user.Id, user.Email, user.UserName!, user.FullName, rolesResult.Data, user.PreferredLanguage);
            return AuthErrors.InvalidCredentials;

        }

        public async Task<Result<UserDataDto>> CreateStaffAsync(CreateStaffCommand command, CancellationToken ct = default)
        {
            var createUserResult = await CreateUserAsync(new RegisterCommand(command.FullName, command.Email, command.Password, command.UserName, command.PreferredLanguage));
            if (!createUserResult.IsSuccess)
                return createUserResult.Error;
            var creteRoleResult = await CreateRoleForUser(command.Email, command.Role, ct);
            if (creteRoleResult.IsSuccess)
                return creteRoleResult.Error;

            var userResult = await GetUserByEmailAsync(command.Email);
            if (!userResult.IsSuccess) return userResult.Error;
            var user = userResult.Data;
            return new UserDataDto(user.Id, user.Email, user.UserName, user.FullName, user.Roles, user.PreferredLanguage);

        }

        public async Task<Result<IReadOnlyList<UserInfoDto>>> GetUsersByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken ct = default)
        {
            var users = await _userManager.Users.AsNoTracking().Where(u => ids.Contains(u.Id)).Select(u => new UserInfoDto(u.Id, u.Email, u.FullName)).ToListAsync(ct);
            return users;

        }

        //public async Task<Result<use>> GetUserFullNameByIdAsync(Guid Id, CancellationToken ct = default)
        //{
        //    var user = await _userManager.FindByIdAsync(Id.ToString());
        //    if (user == null)
        //        return ValidationErrors.Failed;
        //    return user.FullName;
        //}




        //public Task LogoutAsync()
        //{
        //    _userManager.Users.
        //}

    }
}
