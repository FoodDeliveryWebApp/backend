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

        public async Task<RestaurantRating> AddAsync(RestaurantRating rating)
        {
            await _context.RestaurantRatings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<RestaurantRating> GetByIdAsync(int id)
        {
            return await _context.RestaurantRatings
                .Include(r => r.RatedBy)
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.Id == id && !r.isDeleted);
        }

        public async Task UpdateAsync(RestaurantRating rating)
        {
            _context.RestaurantRatings.Update(rating);
            await _context.SaveChangesAsync();
        }



        // Get all ratings for a specific restaurant by its ID
        public async Task<List<RestaurantRating>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.RestaurantRatings
                .Include(r => r.RatedBy)
                .Include(r => r.Restaurant)
                .Where(r => r.Restaurant.Id == restaurantId && !r.isDeleted)
                .ToListAsync();
        }
    }
}