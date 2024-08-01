using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Services;

public class DirectorService : IDirectorService
{
    private readonly MovieStoreContext _context;
    private readonly IMapper _mapper;

    public DirectorService(MovieStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DirectorDto>> GetAllDirectorsAsync()
    {
        var directors = await _context.Directors.ToListAsync();
        return _mapper.Map<IEnumerable<DirectorDto>>(directors);
    }

    public async Task<DirectorDto> GetDirectorByIdAsync(int id)
    {
        var director = await _context.Directors.FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
            throw new KeyNotFoundException("Director not found");

        return _mapper.Map<DirectorDto>(director);
    }

    public async Task<DirectorDto> AddDirectorAsync(DirectorDto directorDto)
    {
        var director = _mapper.Map<Director>(directorDto);
        _context.Directors.Add(director);
        await _context.SaveChangesAsync();
        return _mapper.Map<DirectorDto>(director);
    }

    public async Task<DirectorDto> UpdateDirectorAsync(int id, DirectorDto directorDto)
    {
        var director = await _context.Directors.FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
            throw new KeyNotFoundException("Director not found");

        _mapper.Map(directorDto, director);
        await _context.SaveChangesAsync();
        return _mapper.Map<DirectorDto>(director);
    }

    public async Task DeleteDirectorAsync(int id)
    {
        var director = await _context.Directors.FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
            throw new KeyNotFoundException("Director not found");

        _context.Directors.Remove(director);
        await _context.SaveChangesAsync();
    }
}
