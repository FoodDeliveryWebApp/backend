using Explorer.Stakeholders.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IRatingReportRepository
    {
        Task<RatingReport> CreateAsync(RatingReport report);
        Task<RatingReport> GetByIdAsync(int id);
        Task<List<RatingReport>> GetAllAsync();
        Task<RatingReport> UpdateAsync(RatingReport report);
    }
}
