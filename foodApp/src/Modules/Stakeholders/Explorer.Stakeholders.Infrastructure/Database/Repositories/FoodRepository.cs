using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;  // Added this import

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly StakeholdersContext _dbContext;

        public FoodRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Implementing the GetAllFoodByRestaurantAsync method
        public async Task<IEnumerable<Food>> GetAllFoodByRestaurantAsync(long restaurantId)
        {
            return await _dbContext.Foods
                .Where(food => food.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<List<Food>> GetByIdsAsync(IEnumerable<long> ids)
        {
            return await _dbContext.Foods
                .Where(f => ids.Contains(f.Id))
                .ToListAsync();
        }
    }
}