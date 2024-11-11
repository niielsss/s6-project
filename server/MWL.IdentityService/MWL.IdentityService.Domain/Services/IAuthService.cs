using Microsoft.AspNetCore.Identity;
using MWL.IdentityService.Domain.Entities;
using MWL.IdentityService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.IdentityService.Domain.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterRequest registerRequest, string role);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<User> GetUser(string username);
        Task UpdateRefreshTokenUser(User user);
        Task DeleteUser(string id);
        Task CreateAdmin();
    }
}
