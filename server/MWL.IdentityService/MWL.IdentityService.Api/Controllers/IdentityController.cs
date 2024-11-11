using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MWL.IdentityService.Domain.Entities;
using MWL.IdentityService.Domain.Exceptions;
using MWL.IdentityService.Domain.Models;
using MWL.IdentityService.Domain.Services;

namespace MWL.IdentityService.Api.Controllers
{
    [Route("api/v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAuthService _authService;

        public IdentityController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.Register(registerRequest, UserRoles.User);

                if (result.Succeeded)
                    return CreatedAtAction(nameof(Register), new { username = registerRequest.Username }, registerRequest);

                return BadRequest(result.Errors);
            }
            catch (RegisterError ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loginResponse = await _authService.Login(loginRequest);

                var cookieOptionsAccessToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(35)
                };

                var cookieOptionsRefreshToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                Response.Cookies.Append("access_token", loginResponse.Token, cookieOptionsAccessToken);
                Response.Cookies.Append("refresh_token", loginResponse.RefreshToken, cookieOptionsRefreshToken);

                return Ok(loginResponse);
            }
            catch (ValidationError ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (EntityNotFoundError)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _authService.DeleteUser(id);

                return NoContent();
            }
            catch (EntityNotFoundError)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            try
            {
                await _authService.CreateAdmin();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
