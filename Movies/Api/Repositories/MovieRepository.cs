using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class MovieRepository(IApplicationDbContext context) : IMovieRepository
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<MovieWrapper> GetMovies(QueryParametersDto query)
        {  
            if (query.Page < 1) query.Page = 1;
            if (query.Limit < 1) query.Limit = 10;

            // Start with base query
            var moviesQuery = _context.Movies.AsQueryable();

            // Apply filters
            moviesQuery = ApplyTitleFilter(moviesQuery, query.Title);
            moviesQuery = ApplyGenreFilter(moviesQuery, query.Genre);
            moviesQuery = ApplyReleaseYearFilter(moviesQuery, query.ReleaseYear);

            // Ensure required fields exist
            moviesQuery = moviesQuery.Where(m =>
                !string.IsNullOrEmpty(m.Title) &&
                !string.IsNullOrEmpty(m.Genre));

            // Apply sorting
            moviesQuery = ApplySorting(moviesQuery, query.SortBy, query.SortOrder);

            // Get total count before pagination
            int totalCount = await moviesQuery.CountAsync();

            // Apply pagination
            var movies = await moviesQuery
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
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
                .ToListAsync();

            return new MovieWrapper
            {
                Movies = movies,
                TotalRecords = totalCount,
                SearchParameters = query
            };
        }
         
        private static IQueryable<Movie> ApplyTitleFilter(IQueryable<Movie> query, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return query;

            return query.Where(m => m.Title.Contains(title));
        }

        private static IQueryable<Movie> ApplyGenreFilter(IQueryable<Movie> query, string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
                return query;

            return query.Where(m => m.Genre.Contains(genre));
        }

        private static IQueryable<Movie> ApplyReleaseYearFilter(IQueryable<Movie> query, int? releaseYear)
        {
            if (!releaseYear.HasValue)
                return query;

            return query.Where(m =>
                m.ReleaseDate.HasValue &&
                m.ReleaseDate.Value.Year == releaseYear.Value);
        }
         
        private static IQueryable<Movie> ApplySorting(
            IQueryable<Movie> query,
            string sortBy,
            string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            bool isAscending = !string.IsNullOrWhiteSpace(sortOrder) &&
                              sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return sortBy.ToLowerInvariant() switch
            {
                "title" => isAscending
                    ? query.OrderBy(m => m.Title)
                    : query.OrderByDescending(m => m.Title),
                "releasedate" => isAscending
                    ? query.OrderBy(m => m.ReleaseDate)
                    : query.OrderByDescending(m => m.ReleaseDate),
                _ => query
            };
        }
    }
}
