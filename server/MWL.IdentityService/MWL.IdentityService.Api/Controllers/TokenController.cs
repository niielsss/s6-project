using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MWL.IdentityService.Domain.Models;
using MWL.IdentityService.Domain.Services;

namespace MWL.IdentityService.Api.Controllers
{
    [Route("api/v1/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public TokenController(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var accessToken = Request.Cookies["access_token"];
            var refreshToken = Request.Cookies["refresh_token"];

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = await _authService.GetUser(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
                return BadRequest("Invalid token");

            var newToken = _tokenService.GenerateToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _authService.UpdateRefreshTokenUser(user);

            Response.Cookies.Append("access_token", newToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(65)
            });

            Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            var loginResponse = new LoginResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };

            return Ok(loginResponse);
        }

        [HttpPost("revoke"), Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;

            var user = await _authService.GetUser(username);

            if (user == null)
                return BadRequest("User not found");

            user.RefreshToken = null;

            await _authService.UpdateRefreshTokenUser(user);

            return NoContent();
        }
    }
}
