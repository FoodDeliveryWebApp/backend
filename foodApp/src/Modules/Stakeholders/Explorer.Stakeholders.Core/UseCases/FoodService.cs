using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;

        public FoodService(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Result<IEnumerable<FoodDto>>> GetAllFoodByRestaurantAsync(int restaurantId)
        {
            var foods = await _foodRepository.GetAllFoodByRestaurantAsync(restaurantId);
            if (foods == null || !foods.Any())
                return Result.Fail("No food found for the specified restaurant.");

            var result = foods.Select(f => new FoodDto
            {
                Id = f.Id,
                Name = f.Name,
                Price = f.Price,
                Description = f.Description,
                ImageUrl = f.ImageUrl,
                RestaurantId = f.RestaurantId
            });

            return Result.Ok(result);
        }

        public async Task<Result> UpdateFoodAsync(int foodId, FoodDto dto)
        {
            var food = await _foodRepository.GetByIdAsync(foodId);
            if (food == null) return Result.Fail("Food item not found.");

            food.Update(dto.Name!, dto.Price, dto.Description!, dto.ImageUrl!);
            await _foodRepository.UpdateAsync(food);

            return Result.Ok();
        }

        public async Task<Result> RemoveFoodAsync(int foodId)
        {
            var food = await _foodRepository.GetByIdAsync(foodId);
            if (food == null) return Result.Fail("Food item not found.");

            await _foodRepository.DeleteAsync(foodId);

            return Result.Ok();
        }
    }
}
