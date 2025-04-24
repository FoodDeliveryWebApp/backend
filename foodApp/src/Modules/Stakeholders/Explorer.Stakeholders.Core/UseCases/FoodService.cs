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
    internal class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;

        public FoodService(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        public async Task<Result<IEnumerable<FoodDto>>> GetAllFoodByRestaurantAsync(long restaurantId)
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
    }
}
