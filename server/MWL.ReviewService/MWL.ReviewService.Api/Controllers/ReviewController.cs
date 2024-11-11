using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MWL.ReviewService.Api.DTOs;
using MWL.ReviewService.Domain.Entities;
using MWL.ReviewService.Domain.Exceptions;
using MWL.ReviewService.Domain.Models;
using MWL.ReviewService.Domain.Services;

namespace MWL.ReviewService.Api.Controllers
{
    [Route("api/v1/review")]
    [ApiController]
    public class ReviewController(IReviewService reviewService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationDTO<ReviewResponseDTO>>> Get([FromQuery] ReviewFilterDTO reviewFilterDTO, CancellationToken cancellationToken)
        {
            try
            {
                var filter = mapper.Map<ReviewFilter>(reviewFilterDTO);

                var result = await reviewService.GetListByFilterAsync(filter, cancellationToken);

                var paginationDTO = mapper.Map<PaginationDTO<ReviewResponseDTO>>(result);

                return paginationDTO;
            }
            catch (ValidationError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReviewRequestDTO reviewRequestDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = mapper.Map<Review>(reviewRequestDTO);

            var id = await reviewService.CreateAsync(review, cancellationToken);

            return CreatedAtAction(nameof(Post), new { id }, new { Id = id });
        }
    }
}
