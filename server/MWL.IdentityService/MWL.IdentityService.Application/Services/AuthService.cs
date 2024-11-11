using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MWL.IdentityService.Domain.Entities;
using MWL.IdentityService.Domain.Exceptions;
using MWL.IdentityService.Domain.Models;
using MWL.IdentityService.Domain.Services;
using MWL.IdentityService.Messaging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MWL.IdentityService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> Register(RegisterRequest registerRequest, string role)
        {
            var user = new User
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                throw new RegisterError(result.Errors);

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);

            if (user == null)
                throw new EntityNotFoundError("User not found");

            if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                throw new ValidationError("Invalid password");

            var claims = await GetClaims(user);

            var token = _tokenService.GenerateToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        private async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim("Role", role));
            }

            return claims;
        }

        public async Task<User> GetUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task UpdateRefreshTokenUser(User user)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if (userToUpdate == null)
                throw new EntityNotFoundError("User not found");

            userToUpdate.RefreshToken = user.RefreshToken;

            await _userManager.UpdateAsync(userToUpdate);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new EntityNotFoundError("User not found");

            await _userManager.DeleteAsync(user);

            var message = new BrokerMessage
            {
                Action = "DeleteReviews",
                Data = id
            };

            var reviewsMessaging = new ReviewsMessaging();

            await reviewsMessaging.CallAsync(message);
        }

        public async Task CreateAdmin()
        {
            var user = new User
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            var result = await _userManager.CreateAsync(user, "Password123!");

            if (!result.Succeeded)
                throw new RegisterError(result.Errors);

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
