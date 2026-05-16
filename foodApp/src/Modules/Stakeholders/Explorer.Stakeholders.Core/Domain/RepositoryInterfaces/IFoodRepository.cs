using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllFoodByRestaurantAsync(long restaurantId);
        Task<List<Food>> GetByIdsAsync(IEnumerable<long> ids);
    }
}
