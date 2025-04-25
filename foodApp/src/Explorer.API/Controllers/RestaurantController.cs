using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Explorer.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantController : BaseApiController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllRestaurants()
        {
            var result = await _restaurantService.GetAllRestaurantsAsync();
            return CreateResponse(result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<RestaurantDto>> AddRestaurant([FromBody] RestaurantDto restaurantDto)
        {
            var result = await _restaurantService.AddRestaurantAsync(restaurantDto);
            return Ok(result);
        }


        // Add Worker to a Restaurant
        [HttpPost("{restaurantId}/workers")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> AddWorkerToRestaurant(long restaurantId, [FromBody] UserDto workerDto)
        {
            var result = await _restaurantService.AddWorkerToRestaurantAsync(restaurantId, workerDto);
            return CreateResponse(result);
        }

        // Add Food to a Restaurant
        [HttpPost("{restaurantId}/foods")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> AddFoodToRestaurant(long restaurantId, [FromBody] FoodDto foodDto)
        {
            foodDto.RestaurantId = restaurantId;
            var result = await _restaurantService.AddFoodToRestaurantAsync(foodDto);
            return CreateResponse(result);
        }



    }
}
