using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentResults;
using System.Threading.Tasks;

namespace Explorer.API.Controllers
{
    [Route("api/restaurant-ratings")]
    [ApiController]
    public class RestaurantRatingController : ControllerBase
    {
        private readonly IRestaurantRatingService _ratingService;

        public RestaurantRatingController(IRestaurantRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<RestaurantRatingDto>> AddRating([FromBody] RestaurantRatingDto ratingDto)
        {
            var userIdClaim = User.FindFirstValue("id");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user.");

            ratingDto.RatedByUserId = userId;
            ratingDto.CreatedAt = System.DateTime.UtcNow;

            var result = await _ratingService.AddRatingAsync(ratingDto);

            if (result.IsFailed)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(AddRating), result.Value);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<RestaurantRatingDto>> UpdateRating(int id, [FromBody] RestaurantRatingDto ratingDto)
        {
            var userIdClaim = User.FindFirstValue("id");
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user.");

            ratingDto.RatedByUserId = userId;

            var result = await _ratingService.UpdateRatingAsync(id, ratingDto);

            if (result.IsFailed)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "guest,manager")]
        public async Task<ActionResult<List<RestaurantRatingDto>>> GetRatingsForRestaurant(int restaurantId)
        {
            var ratings = await _ratingService.GetRatingsForRestaurantAsync(restaurantId);

            if (ratings == null || ratings.Count == 0)
                return NotFound("No ratings found for this restaurant.");

            return Ok(ratings);
        }

        [HttpGet("restaurant/{restaurantId}/average")]
        [AllowAnonymous]
        public async Task<ActionResult<double?>> GetAverageRating(int restaurantId)
        {
            var average = await _ratingService.GetAverageRatingAsync(restaurantId);
            return Ok(average);
        }
    }
}
