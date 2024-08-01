using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Services;

public class ActorService : IActorService
{
    private readonly MovieStoreContext _context;
    private readonly IMapper _mapper;

    public ActorService(MovieStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ActorDto>> GetAllActorsAsync()
    {
        var actors = await _context.Actors.ToListAsync();
        return _mapper.Map<IEnumerable<ActorDto>>(actors);
    }

    public async Task<ActorDto> GetActorByIdAsync(int id)
    {
        var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        if (actor == null)
            throw new KeyNotFoundException("Actor not found");

        return _mapper.Map<ActorDto>(actor);
    }

    public async Task<ActorDto> AddActorAsync(ActorDto actorDto)
    {
        var actor = _mapper.Map<Actor>(actorDto);
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();
        return _mapper.Map<ActorDto>(actor);
    }

    public async Task<ActorDto> UpdateActorAsync(int id, ActorDto actorDto)
    {
        var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        if (actor == null)
            throw new KeyNotFoundException("Actor not found");

        _mapper.Map(actorDto, actor);
        await _context.SaveChangesAsync();
        return _mapper.Map<ActorDto>(actor);
    }

    public async Task DeleteActorAsync(int id)
    {
        var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
        if (actor == null)
            throw new KeyNotFoundException("Actor not found");

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();
    }
}
