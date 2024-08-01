using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Services;

public class MovieService : IMovieService
{
    private readonly MovieStoreContext _context;
    private readonly IMapper _mapper;

    public MovieService(MovieStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await _context.Movies
            .Include(m => m.Director)
            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
            .Where(m => m.IsActive)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MovieDto>>(movies);
    }

    public async Task<MovieDto> GetMovieByIdAsync(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Director)
            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
            .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

        if (movie == null)
            throw new KeyNotFoundException("Movie not found");

        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto> AddMovieAsync(MovieDto movieDto)
    {
        var movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return _mapper.Map<MovieDto>(movie);
    }

    public async Task<MovieDto> UpdateMovieAsync(int id, MovieDto movieDto)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
        if (movie == null)
            throw new KeyNotFoundException("Movie not found");

        _mapper.Map(movieDto, movie);
        await _context.SaveChangesAsync();
        return _mapper.Map<MovieDto>(movie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
        if (movie == null)
            throw new KeyNotFoundException("Movie not found");

        movie.IsActive = false;
        await _context.SaveChangesAsync();
    }
}
