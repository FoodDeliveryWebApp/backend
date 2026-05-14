using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly StakeholdersContext _context;

        public RestaurantRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants
                .Include(r => r.Manager)
                .Include(r => r.Workers)
                .Include(r => r.Foods)
                .ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantById(long restaurantId)
        {
            return await _context.Restaurants
                .Include(r => r.Manager)
                .Include(r => r.Workers)
                .Include(r => r.Foods)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);
        }

        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant?> GetByIdAsync(long id)
        {
            return await _context.Restaurants
                .Include(r => r.Manager)
                .Include(r => r.Workers)
                .Include(r => r.Foods)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // Update an existing restaurant
        public async Task UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
    }
}