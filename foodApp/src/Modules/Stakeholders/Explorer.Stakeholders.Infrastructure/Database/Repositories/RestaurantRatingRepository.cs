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
    public class RestaurantRatingRepository : IRestaurantRatingRepository
    {
        private readonly StakeholdersContext _context;

        public RestaurantRatingRepository(StakeholdersContext context)
        {
            _context = context;
        }

        // Add a new restaurant rating
        public async Task AddAsync(RestaurantRating rating)
        {
            await _context.RestaurantRatings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        // Get all ratings for a specific restaurant by its ID
        public async Task<List<RestaurantRating>> GetByRestaurantIdAsync(long restaurantId)
        {
            return await _context.RestaurantRatings
                .Where(r => r.Restaurant.Id == restaurantId)
                .ToListAsync();
        }
    }
}