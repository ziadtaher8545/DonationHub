using DonationHub.Application.Common.Options;
using DonationHub.Application.Dtos;
using DonationHub.Application.Features.Authentication.Login;
using DonationHub.Application.Features.Authentication.Register;
using DonationHub.Application.Interfaces;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly IGenericRepsitory<RefreshToken> _tokenRepo;


        public IdentityService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ITokenService tokenService, IOptions<JwtOptions> options, IGenericRepsitory<RefreshToken> tokenRepo)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _tokenRepo = tokenRepo;
        }

        public async Task<bool> CheckEmailExistAsync(string email, CancellationToken ct = default)
        => await _userManager.FindByEmailAsync(email) is not null;

        public async Task<bool> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IReadOnlyList<string>> GetRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return [];
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        // should return result 
        public async Task<bool> CreateUserAsync(RegisterCommand register, CancellationToken ct = default)
        {
            var user = ApplicationUser.Create(register.FullName, register.Email, register.UserName, register.PreferredLanguage);

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return false;

            }

            await _userManager.AddToRoleAsync(user, "Donor");
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> LoginAsync(LoginCommand login, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                return null;
            }
            var passwordCheck = await CheckPasswordAsync(login.Email, login.Password, ct);
            if (!passwordCheck || !user.IsActive)
                return null;
            var roles = await GetRolesAsync(login.Email, ct);
            var token = _tokenService.CreateToken(user.Id.ToString(), user.Email!, user.UserName!, roles);




            var (plainText, refreshToken) = _tokenService.CreateRefreshToken(user.Id);


            await _tokenRepo.CreateAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync();


            return new UserDto
            {
                Email = user.Email!,
                FullName = user.FullName!,
                AcessToken = token,
                RefershToken = plainText

            };


        }

        public async Task<UserDataDto> GetUserByEmailAsync(string email, CancellationToken ct = default)
        {
            if (email == null)
                return null;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            var roles = await GetRolesAsync(user.Email, ct);
            //var token = _tokenService.CreateToken(user.Id.ToString(), user.Email!, user.UserName!, roles);
            //var refershtoken=await _tokenRepo.g
            return new UserDataDto(user.Email, user.UserName, user.FullName, roles);

        }

        public async Task<bool> CreateRoleForUser(string email, string role, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return false;
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) return false;
            return true;
        }

        public async Task<IdentityUserDto?> GetUserByIdAsync(Guid Id, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user is null) return null;
            var roles = await GetRolesAsync(user.Email, ct);
            return new IdentityUserDto(user.Id, user.Email, user.UserName, user.FullName, roles);



        }

        public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            // must return result
            if (result.Succeeded)
                return true;
            else return false;
        }

        public async Task<UserDataDto> UpdateProfileAsync(string email, string fullName, string preferredLanguage, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            user.ChangeFullName(fullName);
            user.ChangePreferredLanguage(preferredLanguage);
            var roles = await GetRolesAsync(email);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new UserDataDto(email, user.UserName!, user.FullName, roles, user.PreferredLanguage);
            return null;
        }




        //public Task LogoutAsync()
        //{
        //    _userManager.Users.
        //}

    }
}
