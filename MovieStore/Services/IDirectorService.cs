using MovieStore.DTOs;

namespace MovieStore.Services;

public interface IDirectorService
{
    Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync();
    Task<DirectorDto> GetDirectorByIdAsync(int id);
    Task<DirectorDto> AddDirectorAsync(DirectorDto directorDto);
    Task<DirectorDto> UpdateDirectorAsync(int id, DirectorDto directorDto);
    Task DeleteDirectorAsync(int id);
}
