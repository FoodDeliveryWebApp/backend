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
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<RestaurantDto>> AddRestaurant([FromBody] RestaurantDto restaurantDto)
        {
            var result = await _restaurantService.AddRestaurantAsync(restaurantDto);
            return Ok(result);
        }


        // Add Worker to a Restaurant
        [HttpPost("{restaurantId}/workers")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> AddWorkerToRestaurant(int restaurantId, [FromBody] UserDto workerDto)
        {
            var result = await _restaurantService.AddWorkerToRestaurantAsync(restaurantId, workerDto);
            return CreateResponse(result);
        }

        // Add Food to a Restaurant
        [HttpPost("{restaurantId}/foods")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> AddFoodToRestaurant(int restaurantId, [FromBody] FoodDto foodDto)
        {
            foodDto.RestaurantId = restaurantId;
            var result = await _restaurantService.AddFoodToRestaurantAsync(foodDto);
            return CreateResponse(result);
        }

        [HttpGet("{restaurantId}/workers")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetWorkers(int restaurantId)
        {
            var result = await _restaurantService.GetWorkersAsync(restaurantId);
            return CreateResponse(result);
        }

        [HttpDelete("{restaurantId}/workers/{workerId}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> RemoveWorker(int restaurantId, int workerId)
        {
            var result = await _restaurantService.RemoveWorkerAsync(restaurantId, workerId);
            return CreateResponse(result);
        }

        [HttpPut("{restaurantId}/workers/{workerId}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> UpdateWorker(int restaurantId, int workerId, [FromBody] UserDto dto)
        {
            var result = await _restaurantService.UpdateWorkerAsync(restaurantId, workerId, dto);
            return CreateResponse(result);
        }
    }
}
