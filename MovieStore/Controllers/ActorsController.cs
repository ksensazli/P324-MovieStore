using Microsoft.AspNetCore.Mvc;
using MovieStore.DTOs;
using MovieStore.Services;

namespace MovieStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActors()
    {
        var actors = await _actorService.GetAllActorsAsync();
        return Ok(actors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActor(int id)
    {
        var actor = await _actorService.GetActorByIdAsync(id);
        return Ok(actor);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActor([FromBody] ActorDto actorDto)
    {
        var actor = await _actorService.AddActorAsync(actorDto);
        return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, [FromBody] ActorDto actorDto)
    {
        var actor = await _actorService.UpdateActorAsync(id, actorDto);
        return Ok(actor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(int id)
    {
        await _actorService.DeleteActorAsync(id);
        return NoContent();
    }
}
