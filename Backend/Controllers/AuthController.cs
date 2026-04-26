using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var account = await _accountService.AuthenticateAsync(request.Username, request.Password);
        
        if (account == null)
            return Unauthorized(new { message = "Invalid username or password." });

        // Returning user details (no JWT for basic setup)
        return Ok(new { 
            message = "Login successful", 
            account = new { 
                id = account.Id, 
                username = account.Username, 
                email = account.Email, 
                roleId = account.RoleId,
                roleName = account.RoleName
            } 
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingAccount = await _accountService.GetByUsernameAsync(request.Username);
        
        if (existingAccount != null)
            return BadRequest(new { message = "Username already exists." });

        var account = new Account
        {
            Username = request.Username,
            Password = request.Password, // NOTE: In production, hash this password
            Email = request.Email,
            RoleId = 3 // Default Role: Employee (from DbInitializer)
        };

        try
        {
            await _accountService.CreateAsync(account);
            return Ok(new { message = "Registration successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Registration failed. Please check if the email is also unique or if there are other issues.", details = ex.Message });
        }
    }
}
