using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
   public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> GetRestaurantById(long restaurantId);
        Task<Restaurant> Create(Restaurant restaurant);

        Task<Restaurant?> GetByIdAsync(long id);
        Task UpdateAsync(Restaurant restaurant);

    }
}
