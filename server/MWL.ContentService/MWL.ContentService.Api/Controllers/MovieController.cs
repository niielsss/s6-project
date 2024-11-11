using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MWL.ContentService.Api.DTOs;
using MWL.ContentService.Api.Helpers;
using MWL.ContentService.Domain.Entities;
using MWL.ContentService.Domain.Exceptions;
using MWL.ContentService.Domain.Models;
using MWL.ContentService.Domain.Services;

namespace MWL.ContentService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/movie")]
    public class MovieController(IMovieService movieService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationDTO<MovieResponseDTO>>> Get([FromQuery] MovieFilterDTO movieFilterDTO, CancellationToken cancellationToken)
        {
            try
            {
                var filter = mapper.Map<MovieFilter>(movieFilterDTO);

                var result = await movieService.GetListByFilterAsync(filter, cancellationToken);

                var paginationDTO = mapper.Map<PaginationDTO<MovieResponseDTO>>(result);

                return paginationDTO;
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieWithReviewsResponse>> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ControllerHelper.CreateProblemDetails("ID", "Invalid ID"));

                var movieFilterDTO = new MovieFilterDTO { Id = id };

                var filter = mapper.Map<MovieFilter>(movieFilterDTO);

                var result = await movieService.GetByFilterAsync(filter, cancellationToken);

                var response = mapper.Map<MovieWithReviewsResponse>(result);

                if (response is null)
                    return NotFound();

                return response;
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovieRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = mapper.Map<Movie>(requestDTO);

            var id = await movieService.CreateAsync(movie, cancellationToken);

            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MovieRequestDTO requestDTO, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id <= 0)
                    return BadRequest(ControllerHelper.CreateProblemDetails("ID", "Invalid ID"));

                var movie = mapper.Map<Movie>(requestDTO);

                await movieService.UpdateAsync(id, movie, cancellationToken);

                return NoContent();
            }
            catch (EntityNotFoundError ex)
            {
                return NotFound();
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(ControllerHelper.CreateProblemDetails("ID", "Invalid ID"));

                await movieService.DeleteAsync(id, cancellationToken);

                return NoContent();
            }
            catch (EntityNotFoundError ex)
            {
                return NotFound();
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fake")]
        public async Task<ActionResult<string>> Fake()
        {
            try
            {
                return "Fake data created successfully.";
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
