using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;
using MovieStore.Services;
using MovieStore.Tests.Helpers;

namespace MovieStore.Tests;

public class ActorServiceTests
{
    private readonly ActorService _actorService;
    private readonly Mock<MovieStoreContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public ActorServiceTests()
    {
        _contextMock = new Mock<MovieStoreContext>();
        _mapperMock = new Mock<IMapper>();
        _actorService = new ActorService(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllActorsAsync_ReturnsActors()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, FirstName = "Actor 1" },
            new Actor { Id = 2, FirstName = "Actor 2" }
        };

        var actorsDbSetMock = actors.CreateDbSetMock();
        _contextMock.Setup(c => c.Actors).Returns(actorsDbSetMock.Object);

        var actorDtos = new List<ActorDto>
        {
            new ActorDto { Id = 1, FullName = "Actor 1" },
            new ActorDto { Id = 2, FullName = "Actor 2" }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<ActorDto>>(It.IsAny<IEnumerable<Actor>>()))
            .Returns(actorDtos);

        // Act
        var result = await _actorService.GetAllActorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetActorByIdAsync_ReturnsActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, FirstName = "Actor 1" };

        _contextMock.Setup(c => c.Actors.FindAsync(1))
            .ReturnsAsync(actor);

        var actorDto = new ActorDto { Id = 1, FullName = "Actor 1" };

        _mapperMock.Setup(m => m.Map<ActorDto>(It.IsAny<Actor>())).Returns(actorDto);

        // Act
        var result = await _actorService.GetActorByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AddActorAsync_AddsActor()
    {
        // Arrange
        var actorDto = new ActorDto { FullName = "New Actor" };
        var actor = new Actor { Id = 1, FirstName = "New Actor" };

        _mapperMock.Setup(m => m.Map<Actor>(It.IsAny<ActorDto>())).Returns(actor);
        _contextMock.Setup(c => c.Actors.Add(actor)).Verifiable();
        _mapperMock.Setup(m => m.Map<ActorDto>(actor)).Returns(actorDto);

        // Act
        var result = await _actorService.AddActorAsync(actorDto);

        // Assert
        _contextMock.Verify(c => c.Actors.Add(actor), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(actorDto.FullName, result.FullName);
    }

    [Fact]
    public async Task UpdateActorAsync_UpdatesActor()
    {
        // Arrange
        var actorDto = new ActorDto { FullName = "Updated Actor" };
        var actor = new Actor { Id = 1, FirstName = "Old Actor" };

        _contextMock.Setup(c => c.Actors.FirstOrDefaultAsync(a => a.Id == 1, default))
            .ReturnsAsync(actor);

        _mapperMock.Setup(m => m.Map(actorDto, actor));

        // Act
        var result = await _actorService.UpdateActorAsync(1, actorDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(actorDto.FullName, result.FullName);
    }

    [Fact]
    public async Task DeleteActorAsync_RemovesActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, FirstName = "Actor 1" };

        _contextMock.Setup(c => c.Actors.FirstOrDefaultAsync(a => a.Id == 1, default))
            .ReturnsAsync(actor);

        // Act
        await _actorService.DeleteActorAsync(1);

        // Assert
        _contextMock.Verify(c => c.Actors.Remove(actor), Times.Once);
    }
}