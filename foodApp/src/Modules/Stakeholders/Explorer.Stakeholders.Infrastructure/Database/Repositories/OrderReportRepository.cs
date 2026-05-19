using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class OrderReportRepository : IOrderReportRepository
    {
        private readonly StakeholdersContext _context;

        public OrderReportRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public async Task<OrderReport> CreateAsync(OrderReport report)
        {
            await _context.OrderReports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<List<OrderReport>> GetAllAsync()
        {
            return await _context.OrderReports.ToListAsync();
        }

        public async Task<List<OrderReport>> GetByGuestIdAsync(int guestId)
        {
            return await _context.OrderReports
                .Where(r => r.GuestId == guestId)
                .ToListAsync();
        }

        public async Task<OrderReport?> GetByIdAsync(int id)
        {
            return await _context.OrderReports.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<OrderReport> UpdateAsync(OrderReport report)
        {
            _context.OrderReports.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
