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

        [HttpPut("{id}/delivery-fee")]
        [Authorize(Roles = "administrator,manager")]
        public async Task<ActionResult> UpdateDeliveryFee(int id, [FromBody] int deliveryFee)
        {
            var result = await _restaurantService.UpdateDeliveryFeeAsync(id, deliveryFee);
            return CreateResponse(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<RestaurantDto>> UpdateRestaurant(int id, [FromBody] RestaurantDto dto)
        {
            var result = await _restaurantService.UpdateRestaurantAsync(id, dto);
            return CreateResponse(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id);
            return CreateResponse(result);
        }

        [HttpPost("upload-image")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<string>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image provided.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Only jpg, png, webp and gif files are allowed.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "restaurants");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return Ok($"images/restaurants/{fileName}");
        }
    }
}
