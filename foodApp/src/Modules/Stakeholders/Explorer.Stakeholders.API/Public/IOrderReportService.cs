using Explorer.Stakeholders.API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IOrderReportService
    {
        Task<OrderReportDto> CreateReportAsync(int guestId, OrderReportDto dto);
        Task<List<OrderReportDto>> GetAllReportsAsync();
        Task<List<OrderReportDto>> GetGuestReportsAsync(int guestId);
        Task<OrderReportDto> AnswerReportAsync(int reportId, string answer);
    }
}
