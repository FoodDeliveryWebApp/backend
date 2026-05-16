using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class RestaurantApplicationRepository : IRestaurantApplicationRepository
    {
        private readonly StakeholdersContext _context;

        public RestaurantApplicationRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public async Task<RestaurantApplication> CreateAsync(RestaurantApplication application)
        {
            await _context.RestaurantApplications.AddAsync(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<RestaurantApplication?> GetByIdAsync(int id)
        {
            return await _context.RestaurantApplications.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<RestaurantApplication>> GetAllAsync()
        {
            return await _context.RestaurantApplications.ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetByStatusAsync(ApplicationStatus status)
        {
            return await _context.RestaurantApplications
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<RestaurantApplication> UpdateAsync(RestaurantApplication application)
        {
            _context.RestaurantApplications.Update(application);
            await _context.SaveChangesAsync();
            return application;
        }
    }
}
