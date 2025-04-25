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

        // Get all restaurants
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        // Get a restaurant by its ID
        public async Task<Restaurant> GetRestaurantById(long restaurantId)
        {
            return await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId);
        }

        // Create a new restaurant
        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        // Get a restaurant by its ID with asynchronous method
        public async Task<Restaurant?> GetByIdAsync(long id)
        {
            return await _context.Restaurants
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