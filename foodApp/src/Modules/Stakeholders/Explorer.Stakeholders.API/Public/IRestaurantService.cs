using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IRestaurantService
    {
        Task<Result<IEnumerable<RestaurantDto>>> GetAllRestaurantsAsync();
        Task<RestaurantDto> AddRestaurantAsync(RestaurantDto dto);
        Task<Result> AddWorkerToRestaurantAsync(long restaurantId, UserDto workerDto);
        Task<Result> AddFoodToRestaurantAsync(FoodDto foodDto);



    }
}
