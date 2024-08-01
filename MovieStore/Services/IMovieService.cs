using MovieStore.DTOs;

namespace MovieStore.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<MovieDto> GetMovieByIdAsync(int id);
    Task<MovieDto> AddMovieAsync(MovieDto movieDto);
    Task<MovieDto> UpdateMovieAsync(int id, MovieDto movieDto);
    Task DeleteMovieAsync(int id);
}
