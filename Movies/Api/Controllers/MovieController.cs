using Api.Dtos;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController(IMovieRepository repository) : BaseController
    {
        private readonly IMovieRepository _repository = repository;

        [HttpGet("search")]
        public async Task<IActionResult> GetMovies([FromQuery] QueryParametersDto queryParametersDto)
        {
            var result = await _repository.GetMovies(queryParametersDto);

            return result.Movies != null && result.Movies.Any()
                ? Success(result, "Movies retrieved successfully.")
                : Failure<MovieWrapper>("No movies found matching your criteria.");
        }
    }
}
