using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFoodByRestaurant(int restaurantId)
        {
            var result = await _foodService.GetAllFoodByRestaurantAsync(restaurantId);
            return CreateResponse(result);
        }

        [HttpPut("{foodId}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> UpdateFood(int foodId, [FromBody] FoodDto dto)
        {
            var result = await _foodService.UpdateFoodAsync(foodId, dto);
            return CreateResponse(result);
        }

        [HttpDelete("{foodId}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> RemoveFood(int foodId)
        {
            var result = await _foodService.RemoveFoodAsync(foodId);
            return CreateResponse(result);
        }

        [HttpPost("upload-image")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<string>> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image provided.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Only jpg, png, webp and gif files are allowed.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "foods");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            return Ok($"images/foods/{fileName}");
        }
    }
}
