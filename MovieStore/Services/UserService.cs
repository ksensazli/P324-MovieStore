using Microsoft.AspNetCore.Identity;
using MovieStore.DTOs;

namespace MovieStore.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public UserService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<UserDto> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null; // Authentication failed
        }

        var token = _jwtService.GenerateJwtToken(user);
        return new UserDto
        {
            Username = user.UserName,
            Token = token
        };
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser { UserName = registerDto.Username, Email = registerDto.Email };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return null; // Registration failed
        }

        var token = _jwtService.GenerateJwtToken(user);
        return new UserDto
        {
            Username = user.UserName,
            Token = token
        };
    }
}