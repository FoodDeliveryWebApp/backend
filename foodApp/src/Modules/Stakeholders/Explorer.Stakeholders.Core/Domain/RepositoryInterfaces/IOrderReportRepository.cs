using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IOrderReportRepository
    {
        Task<OrderReport> CreateAsync(OrderReport report);
        Task<List<OrderReport>> GetAllAsync();
        Task<List<OrderReport>> GetByGuestIdAsync(int guestId);
        Task<OrderReport?> GetByIdAsync(int id);
        Task<OrderReport> UpdateAsync(OrderReport report);
    }
}
