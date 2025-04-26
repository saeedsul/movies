using Moq;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using Api.Repositories;
using Test.Helpers;
using Api.Dtos;
using Api.Helpers;

namespace Test.Controllers
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieRepository> _repositoryMock;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _repositoryMock = new Mock<IMovieRepository>();
            _controller = new MovieController(_repositoryMock.Object);
        }

        
        [Theory]
        [InlineData("Spider-Man", "Action", 10, 1)]
        [InlineData(null, "Fantasy", 5, 1)]
        [InlineData("Spider", null, 10, 2)]
        [InlineData(null, "Comedy", 10, 1)]
        public async Task GetMovies_ReturnsFilteredResults(string? title, string? genre, int limit, int page)
        {
            // Arrange
            var allMovies = DataHelper.GetFakeMovieList();

            var filteredMovies = allMovies
                .Where(m =>
                    (string.IsNullOrEmpty(title) || m.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(genre) || m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            var expectedDtos = filteredMovies
                .Select(m => new MovieDto
                {
                    Title = m.Title,
                    Overview = m.Overview,
                    ReleaseDate = m.ReleaseDate,
                    Genre = m.Genre,
                    PosterUrl = m.PosterUrl,
                    VoteAverage = m.VoteAverage,
                    VoteCount = m.VoteCount,
                    OriginalLanguage = m.OriginalLanguage,
                    Popularity = m.Popularity
                })
                .ToList();

            var query = new QueryParametersDto
            {
                Title = title,
                Genre = genre,
                Limit = limit,
                Page = page
            };

            var movieWrapper = new MovieWrapper
            {
                Movies = expectedDtos,
                TotalRecords = expectedDtos.Count,
                SearchParameters = query
            };

            _repositoryMock
                .Setup(r => r.GetMovies(It.Is<QueryParametersDto>(q =>
                    q.Title == query.Title &&
                    q.Genre == query.Genre &&
                    q.Limit == query.Limit &&
                    q.Page == query.Page)))
                .ReturnsAsync(movieWrapper);

            // Act
            var result = await _controller.GetMovies(query) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = Assert.IsType<ApiResponse<MovieWrapper>>(result.Value);
            Assert.True(response.Success);
            Assert.Equal("Movies retrieved successfully.", response.Message);
            Assert.Equal(expectedDtos.Count, response.Data.Movies.Count());
        }

    
         

        [Theory]
        [InlineData("Nonexistent", "Nonexistent", 10, 1)] // Filters that match nothing
        [InlineData(null, null, -5, 1)]   // Negative limit
        [InlineData("Spider", "Action", int.MaxValue, 1)] // Extremely high limit
        [InlineData("Spider", "Action", 10, int.MaxValue)] // Extremely high page number
        [InlineData(null, null, 10, 0)]                  // Page number is zero
        [InlineData(null, null, 10, -2)]                 // Negative page number
        public async Task GetMovies_ReturnsFailure_WhenNoMoviesFound(string? title, string? genre, int limit, int page)
        {
            // Arrange
            var query = new QueryParametersDto
            {
                Title = "NonExistentMovie",
                Genre = "Fantasy"
            };

            var emptyResult = new MovieWrapper
            {
                Movies = Enumerable.Empty<MovieDto>(),
                TotalRecords = 0,
                SearchParameters = query
            };

            _repositoryMock
                .Setup(r => r.GetMovies(query))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.GetMovies(query) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var response = Assert.IsType<ApiResponse<MovieWrapper>>(result.Value);
            Assert.False(response.Success);
            Assert.Equal("No movies found matching your criteria.", response.Message);
            Assert.Null(response.Data);
        }

        [Theory] 
        [InlineData("Spider", null, 2002, 10, 2)]
        [InlineData(null, "Documentary", 2002, 10, 1)]
        public async Task GetMovies_ReturnsFilteredResults_WithReleaseYear(string? title, string? genre, int releaseYear, int limit, int page)
        {
            // Arrange
            var allMovies = DataHelper.GetFakeMovieList();

            var filteredMovies = allMovies
                .Where(m =>
                    (string.IsNullOrEmpty(title) || m.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(genre) || m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase)) &&
                    (m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year == releaseYear))
                .ToList();
            //  moviesQuery = moviesQuery.Where(m => m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year == query.ReleaseYear.Value);
            var expectedDtos = filteredMovies
                .Select(m => new MovieDto
                {
                    Title = m.Title,
                    Overview = m.Overview,
                    ReleaseDate = m.ReleaseDate,
                    Genre = m.Genre,
                    PosterUrl = m.PosterUrl,
                    VoteAverage = m.VoteAverage,
                    VoteCount = m.VoteCount,
                    OriginalLanguage = m.OriginalLanguage,
                    Popularity = m.Popularity
                })
                .ToList();

            var query = new QueryParametersDto
            {
                Title = title,
                Genre = genre,
                ReleaseYear = releaseYear,
                Limit = limit,
                Page = page
            };

            var movieWrapper = new MovieWrapper
            {
                Movies = expectedDtos,
                TotalRecords = expectedDtos.Count,
                SearchParameters = query
            };

            _repositoryMock
                .Setup(r => r.GetMovies(It.Is<QueryParametersDto>(q =>
                    q.Title == query.Title &&
                    q.Genre == query.Genre &&
                    q.ReleaseYear == query.ReleaseYear &&
                    q.Limit == query.Limit &&
                    q.Page == query.Page)))
                .ReturnsAsync(movieWrapper);

            // Act
            var result = await _controller.GetMovies(query) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = Assert.IsType<ApiResponse<MovieWrapper>>(result.Value);
            Assert.True(response.Success);
            Assert.Equal("Movies retrieved successfully.", response.Message);
            Assert.Equal(expectedDtos.Count, response.Data.Movies.Count());
        }


    }
}
