using Api.Dtos; 

namespace Api.Repositories
{
    public interface IMovieRepository
    { 
        Task<MovieWrapper> GetMovies(QueryParametersDto query); 
    }
}
