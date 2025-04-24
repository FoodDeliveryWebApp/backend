using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Route("api/foods")]
    [ApiController]
    public class FoodController : BaseApiController
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFoodByRestaurant(long restaurantId)
        {
            var result = await _foodService.GetAllFoodByRestaurantAsync(restaurantId);
            return CreateResponse(result);
        }
    }
}
