using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IRestaurantRatingService
    {
        Task<Result<RestaurantRatingDto>> AddRatingAsync(RestaurantRatingDto dto);
        Task<Result<RestaurantRatingDto>> UpdateRatingAsync(int id, RestaurantRatingDto dto);
        Task<List<RestaurantRatingDto>> GetRatingsForRestaurantAsync(int restaurantId);
        Task<double?> GetAverageRatingAsync(int restaurantId);
    }
}