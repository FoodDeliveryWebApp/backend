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

        // POST api/restaurant-ratings
        [HttpPost]
        [Authorize(Roles = "Guest")]  // Only Guest users can add ratings
        public async Task<ActionResult> AddRating([FromBody] RestaurantRatingDto ratingDto)
        {
            // Optionally, override RatedByUserId from the authenticated user
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }
            //obrati paznju imas i user id u dto
            ratingDto.RatedByUserId = userId;
            ratingDto.CreatedAt = System.DateTime.UtcNow;

            var result = await _ratingService.AddRatingAsync(ratingDto);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(AddRating), null); // No specific resource URL yet
        }


        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "Manager")]  // Only Manager can view ratings
        public async Task<ActionResult<List<RestaurantRatingDto>>> GetRatingsForRestaurant(int restaurantId)
        {
            // Pretpostavljamo da servis ima metodu za dobijanje svih ocena restorana
            var ratings = await _ratingService.GetRatingsForRestaurantAsync(restaurantId);

            if (ratings == null || ratings.Count == 0)
            {
                return NotFound("No ratings found for this restaurant.");
            }

            return Ok(ratings);
        }



    }
}
