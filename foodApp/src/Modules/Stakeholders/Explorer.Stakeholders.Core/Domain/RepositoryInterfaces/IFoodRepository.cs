using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllFoodByRestaurantAsync(int restaurantId);
        Task<List<Food>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Food?> GetByIdAsync(int id);
        Task UpdateAsync(Food food);
        Task DeleteAsync(int id);
    }
}
