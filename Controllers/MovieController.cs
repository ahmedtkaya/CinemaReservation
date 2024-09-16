using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using cinemaReservation.Models;
using cinemaReservation.Services;
using Microsoft.Extensions.Logging;

namespace cinemaReservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [Authorize]//bununla beraber auth olmayan kişi ekleme yapamıyor
        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Director) || string.IsNullOrEmpty(movie.ReleaseYear))
            {
                _logger.LogWarning("Invalid movie information sent");
                return BadRequest("Movie Data is invalid");
            }

            try
            {
                await _movieService.CreateAsync(movie);
                _logger.LogInformation("New movie created successfully: {MovieTitle}", movie.Title);
                return Ok("Movie created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating movie");
                return StatusCode(500, "Internal server error");

            }

        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            if (movies == null || !movies.Any())
            {
                _logger.LogWarning("No movies found.");
                return NotFound("No movies found.");
            }
            _logger.LogInformation("Movies retrieved successfully");
            return Ok(movies);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Movie updatedMovie)
        {
            if (updatedMovie == null || string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid update request");
                return BadRequest("Invalid data provided.");
            }

            try
            {
                await _movieService.UpdateAsync(id, updatedMovie);
                _logger.LogInformation("Movie updated successfully: {Movietitle}", updatedMovie.Title);
                return Ok("Movie updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating movie");
                return StatusCode(500, "Internal Server error");

            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _movieService.DeleteAsync(id);
                _logger.LogInformation("Movie Deleted: {MovieId}", id);
                return Ok("Movie deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while deleting movie ID: {MovieId}", id);
                return NotFound("Movie not found");
            }
        }
    }
}