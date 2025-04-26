using Api.Data;
using Api.Repositories;
using Moq;
using MockQueryable;
using MockQueryable.Moq;
using Test.Helpers;
using Api.Dtos;
using System.Collections.Generic;

namespace Test.Repositories
{
    public class MovieRepositoryTest
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly MovieRepository _sut;

        public MovieRepositoryTest()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _sut = new MovieRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetMovies_FiltersByGenre_ReturnsExpectedResults()
        {
            // Arrange
            var genreFilter = "Action";
            var allMovies = DataHelper.GetFakeMovieList(); // Include different genres here

            var mockDbSet = allMovies.BuildMock().BuildMockDbSet();
            _dbContextMock.SetupGet(db => db.Movies).Returns(mockDbSet.Object);

            var query = new QueryParametersDto
            {
                Genre = genreFilter,
                Limit = 10,
                Page = 1,
                SortBy = "Title",
                SortOrder = "asc"
            };

            var expected = allMovies
                .Where(m =>
                    !string.IsNullOrEmpty(m.Title) &&
                    !string.IsNullOrEmpty(m.Genre) &&
                    m.Genre.Contains(genreFilter, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.Title)
                .Take(10)
                .ToList();

            // Act
            var result = await _sut.GetMovies(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Movies);
           // Assert.Equal(expected.Count, result.Movies.Count);
            Assert.All(result.Movies, m => Assert.Contains(genreFilter, m.Genre, StringComparison.OrdinalIgnoreCase));
        }



        [Fact]
        public async Task GetMovies_FiltersByReleaseYear_ReturnsExpectedMovies()
        {
            // Arrange
            var releaseYear = 2022;
            var allMovies = DataHelper.GetFakeMovieList(); 

            // Mock DbSet
            var mock = allMovies.BuildMock().BuildMockDbSet();
            _dbContextMock.SetupGet(m => m.Movies).Returns(mock.Object);

            var query = new QueryParametersDto
            {
                ReleaseYear = releaseYear,
                Limit = 10,
                Page = 1,
                SortBy = "Title",
                SortOrder = "asc"
            };

            var expected = allMovies
                .Where(m =>
                    !string.IsNullOrEmpty(m.Title) &&
                    !string.IsNullOrEmpty(m.Genre) &&
                    m.ReleaseDate.HasValue &&
                    m.ReleaseDate.Value.Year == releaseYear)
                .OrderBy(m => m.Title)
                .Take(10)
                .ToList();

            // Act
            var result = await _sut.GetMovies(query);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Movies);
            Assert.Equal(expected.Count, result.Movies.ToList().Count);
            Assert.All(result.Movies, m => Assert.Equal(releaseYear, m.ReleaseDate?.Year));
        }


    }
}
