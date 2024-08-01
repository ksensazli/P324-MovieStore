using MovieStore.DTOs;

namespace MovieStore.Services;

public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllActorsAsync();
    Task<ActorDto> GetActorByIdAsync(int id);
    Task<ActorDto> AddActorAsync(ActorDto actorDto);
    Task<ActorDto> UpdateActorAsync(int id, ActorDto actorDto);
    Task DeleteActorAsync(int id);
}
