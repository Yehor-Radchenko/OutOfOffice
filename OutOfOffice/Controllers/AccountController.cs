using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.BLL.Services.Jwt;
using OutOfOffice.Common.Dto;
using OutOfOffice.Common.ResponseModels;
using OutOfOffice.DAL.Models;

namespace OutOfOffice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<Employee> _signInManager;
    private readonly JwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(SignInManager<Employee> signInManager, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid login data." });
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid login attempt." });
            }
            var token = await _jwtService.GenerateToken(user);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            AuthResponse authResponse = new AuthResponse()
            {
                Token = token,
            };

            return Ok(authResponse);
        }
        else if (result.IsLockedOut)
        {
            return StatusCode(403, new { Message = "Account locked out. Please contact support." });
        }
        else if (result.RequiresTwoFactor)
        {
            return BadRequest(new { Message = "Two-factor authentication required." });
        }
        else
        {
            return Unauthorized(new { Message = "Invalid login attempt." });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt-token");
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
        return Ok("Logged out successfully.");
    }
}
