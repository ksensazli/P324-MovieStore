using Microsoft.AspNetCore.Mvc;
using MovieStore.DTOs;
using MovieStore.Services;

namespace MovieStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _userService.RegisterAsync(registerDto);
        if (user == null)
        {
            return BadRequest("Registration failed.");
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RegisterDto loginDto)
    {
        var user = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
}