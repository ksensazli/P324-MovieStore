using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;
using MovieStore.Services;
using MovieStore.Tests.Helpers;

namespace MovieStore.Tests;

public class DirectorServiceTests
{
    private readonly DirectorService _directorService;
    private readonly Mock<MovieStoreContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public DirectorServiceTests()
    {
        _contextMock = new Mock<MovieStoreContext>();
        _mapperMock = new Mock<IMapper>();
        _directorService = new DirectorService(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllDirectorsAsync_ReturnsDirectors()
    {
        // Arrange
        var directors = new List<Director>
        {
            new Director { Id = 1, FirstName = "Director 1" },
            new Director { Id = 2, FirstName = "Director 2" }
        };

        var directorsDbSetMock = directors.CreateDbSetMock();
        _contextMock.Setup(c => c.Directors).Returns(directorsDbSetMock.Object);

        var directorDtos = new List<DirectorDto>
        {
            new DirectorDto { Id = 1, FullName = "Director 1" },
            new DirectorDto { Id = 2, FullName = "Director 2" }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<DirectorDto>>(It.IsAny<IEnumerable<Director>>()))
            .Returns(directorDtos);

        // Act
        var result = await _directorService.GetAllDirectorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetDirectorByIdAsync_ReturnsDirector()
    {
        // Arrange
        var director = new Director { Id = 1, FirstName = "Director 1" };

        _contextMock.Setup(c => c.Directors.FindAsync(1))
            .ReturnsAsync(director);

        var directorDto = new DirectorDto { Id = 1, FullName = "Director 1" };

        _mapperMock.Setup(m => m.Map<DirectorDto>(It.IsAny<Director>())).Returns(directorDto);

        // Act
        var result = await _directorService.GetDirectorByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AddDirectorAsync_AddsDirector()
    {
        // Arrange
        var directorDto = new DirectorDto { FullName = "New Director" };
        var director = new Director { Id = 1, FirstName = "New Director" };

        _mapperMock.Setup(m => m.Map<Director>(It.IsAny<DirectorDto>())).Returns(director);
        _contextMock.Setup(c => c.Directors.Add(director)).Verifiable();
        _mapperMock.Setup(m => m.Map<DirectorDto>(director)).Returns(directorDto);

        // Act
        var result = await _directorService.AddDirectorAsync(directorDto);

        // Assert
        _contextMock.Verify(c => c.Directors.Add(director), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(directorDto.FullName, result.FullName);
    }

    [Fact]
    public async Task UpdateDirectorAsync_UpdatesDirector()
    {
        // Arrange
        var directorDto = new DirectorDto { FullName = "Updated Director" };
        var director = new Director { Id = 1, FirstName = "Old Director" };

        _contextMock.Setup(c => c.Directors.FirstOrDefaultAsync(d => d.Id == 1, default))
            .ReturnsAsync(director);

        _mapperMock.Setup(m => m.Map(directorDto, director));

        // Act
        var result = await _directorService.UpdateDirectorAsync(1, directorDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(directorDto.FullName, result.FullName);
    }

    [Fact]
    public async Task DeleteDirectorAsync_RemovesDirector()
    {
        // Arrange
        var director = new Director { Id = 1, FirstName = "Director 1" };

        _contextMock.Setup(c => c.Directors.FirstOrDefaultAsync(d => d.Id == 1, default))
            .ReturnsAsync(director);

        // Act
        await _directorService.DeleteDirectorAsync(1);

        // Assert
        _contextMock.Verify(c => c.Directors.Remove(director), Times.Once);
    }
}