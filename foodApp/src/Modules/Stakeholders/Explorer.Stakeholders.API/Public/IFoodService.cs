using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IFoodService
    {
        Task<Result<IEnumerable<FoodDto>>> GetAllFoodByRestaurantAsync(int restaurantId);
        Task<Result> UpdateFoodAsync(int foodId, FoodDto dto);
        Task<Result> RemoveFoodAsync(int foodId);
    }
}
