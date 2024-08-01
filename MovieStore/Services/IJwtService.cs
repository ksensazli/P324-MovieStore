using MovieStore.DTOs;

namespace MovieStore.Services;

public interface IJwtService
{
    string GenerateJwtToken(ApplicationUser user);
}