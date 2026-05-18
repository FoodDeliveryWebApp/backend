using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IRestaurantRatingRepository
    {
        Task<RestaurantRating> AddAsync(RestaurantRating rating);
        Task<List<RestaurantRating>> GetByRestaurantIdAsync(int restaurantId);
        Task<RestaurantRating> GetByIdAsync(int id);
        Task UpdateAsync(RestaurantRating rating);
    }
}
