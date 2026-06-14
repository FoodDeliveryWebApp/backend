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
        Task<Result> AddWorkerToRestaurantAsync(int restaurantId, UserDto workerDto);
        Task<Result> AddFoodToRestaurantAsync(FoodDto foodDto);
        Task<Result<IEnumerable<UserDto>>> GetWorkersAsync(int restaurantId);
        Task<Result> RemoveWorkerAsync(int restaurantId, int workerId);
        Task<Result> UpdateWorkerAsync(int restaurantId, int workerId, UserDto dto);
        Task<Result<RestaurantDto>> UpdateRestaurantAsync(int id, RestaurantDto dto);
        Task<Result> UpdateDeliveryFeeAsync(int restaurantId, int deliveryFee);
        Task<Result> DeleteRestaurantAsync(int id);
    }
}
