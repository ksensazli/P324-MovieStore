using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;
using MovieStore.Services;
using MovieStore.Tests.Helpers;

namespace MovieStore.Tests;

public class MovieServiceTests
{
    private readonly MovieService _movieService;
    private readonly Mock<MovieStoreContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public MovieServiceTests()
    {
        _contextMock = new Mock<MovieStoreContext>();
        _mapperMock = new Mock<IMapper>();
        _movieService = new MovieService(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllMoviesAsync_ReturnsMovies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1" },
            new Movie { Id = 2, Title = "Movie 2" }
        };

        var moviesDbSetMock = movies.CreateDbSetMock();
        _contextMock.Setup(c => c.Movies).Returns(moviesDbSetMock.Object);

        var movieDtos = new List<MovieDto>
        {
            new MovieDto { Id = 1, Title = "Movie 1" },
            new MovieDto { Id = 2, Title = "Movie 2" }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<MovieDto>>(It.IsAny<IEnumerable<Movie>>()))
            .Returns(movieDtos);

        // Act
        var result = await _movieService.GetAllMoviesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetMovieByIdAsync_ReturnsMovie()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Movie 1", IsActive = true };

        _contextMock.Setup(c => c.Movies.FindAsync(1))
            .ReturnsAsync(movie);

        var movieDto = new MovieDto { Id = 1, Title = "Movie 1" };

        _mapperMock.Setup(m => m.Map<MovieDto>(It.IsAny<Movie>())).Returns(movieDto);

        // Act
        var result = await _movieService.GetMovieByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AddMovieAsync_AddsMovie()
    {
        // Arrange
        var movieDto = new MovieDto { Title = "New Movie" };
        var movie = new Movie { Id = 1, Title = "New Movie" };

        _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<MovieDto>())).Returns(movie);
        _contextMock.Setup(c => c.Movies.Add(movie)).Verifiable();
        _mapperMock.Setup(m => m.Map<MovieDto>(movie)).Returns(movieDto);

        // Act
        var result = await _movieService.AddMovieAsync(movieDto);

        // Assert
        _contextMock.Verify(c => c.Movies.Add(movie), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(movieDto.Title, result.Title);
    }

    [Fact]
    public async Task UpdateMovieAsync_UpdatesMovie()
    {
        // Arrange
        var movieDto = new MovieDto { Title = "Updated Movie" };
        var movie = new Movie { Id = 1, Title = "Old Movie", IsActive = true };

        _contextMock.Setup(c => c.Movies.FirstOrDefaultAsync(m => m.Id == 1 && m.IsActive, default))
            .ReturnsAsync(movie);

        _mapperMock.Setup(m => m.Map(movieDto, movie));

        // Act
        var result = await _movieService.UpdateMovieAsync(1, movieDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(movieDto.Title, result.Title);
    }

    [Fact]
    public async Task DeleteMovieAsync_MarksMovieAsInactive()
    {
        // Arrange
        var movie = new Movie { Id = 1, Title = "Movie 1", IsActive = true };

        _contextMock.Setup(c => c.Movies.FirstOrDefaultAsync(m => m.Id == 1 && m.IsActive, default))
            .ReturnsAsync(movie);

        // Act
        await _movieService.DeleteMovieAsync(1);

        // Assert
        Assert.False(movie.IsActive);
    }
}
