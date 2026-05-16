using Explorer.Stakeholders.API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IRatingReportService
    {
        Task<RatingReportDto> CreateReportAsync(RatingReportDto dto);
        Task<RatingReportDto> UpdateReportStatusAsync(int reportId, string newStatus);
        Task<List<RatingReportDto>> GetAllReportsAsync();
    }
}
