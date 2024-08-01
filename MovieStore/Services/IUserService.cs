using MovieStore.DTOs;

namespace MovieStore.Services;

public interface IUserService
{
    Task<UserDto> AuthenticateAsync(string username, string password);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
}