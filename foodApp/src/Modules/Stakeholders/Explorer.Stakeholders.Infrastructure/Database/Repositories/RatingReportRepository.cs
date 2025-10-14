using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class RatingReportRepository : IRatingReportRepository
    {
        private readonly StakeholdersContext _context;

        public RatingReportRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public async Task<RatingReport> CreateAsync(RatingReport report)
        {
            await _context.RatingReports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<RatingReport> GetByIdAsync(long id)
        {
            return await _context.RatingReports
                .Include(r => r.Order)
                .Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RatingReport>> GetAllAsync()
        {
            return await _context.RatingReports
                .Include(r => r.Order)
                .Include(r => r.Manager)
                .ToListAsync();
        }

        public async Task<RatingReport> UpdateAsync(RatingReport report)
        {
            _context.RatingReports.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
