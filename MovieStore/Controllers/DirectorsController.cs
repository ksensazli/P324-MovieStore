using Microsoft.AspNetCore.Mvc;
using MovieStore.DTOs;
using MovieStore.Services;

namespace MovieStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController : ControllerBase
{
    private readonly IDirectorService _directorService;

    public DirectorsController(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDirectors()
    {
        var directors = await _directorService.GetAllDirectorsAsync();
        return Ok(directors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDirector(int id)
    {
        var director = await _directorService.GetDirectorByIdAsync(id);
        return Ok(director);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDirector([FromBody] DirectorDto directorDto)
    {
        var director = await _directorService.AddDirectorAsync(directorDto);
        return CreatedAtAction(nameof(GetDirector), new { id = director.Id }, director);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDirector(int id, [FromBody] DirectorDto directorDto)
    {
        var director = await _directorService.UpdateDirectorAsync(id, directorDto);
        return Ok(director);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDirector(int id)
    {
        await _directorService.DeleteDirectorAsync(id);
        return NoContent();
    }
}
