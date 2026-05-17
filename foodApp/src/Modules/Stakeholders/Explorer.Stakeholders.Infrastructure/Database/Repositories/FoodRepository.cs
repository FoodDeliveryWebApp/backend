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
        public async Task<IEnumerable<Food>> GetAllFoodByRestaurantAsync(int restaurantId)
        {
            return await _dbContext.Foods
                .Where(food => food.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<List<Food>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbContext.Foods
                .Where(f => ids.Contains(f.Id))
                .ToListAsync();
        }

        public async Task<Food?> GetByIdAsync(int id)
        {
            return await _dbContext.Foods.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task UpdateAsync(Food food)
        {
            _dbContext.Foods.Update(food);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var food = await _dbContext.Foods.FindAsync(id);
            if (food == null) return;
            _dbContext.Foods.Remove(food);
            await _dbContext.SaveChangesAsync();
        }
    }
}