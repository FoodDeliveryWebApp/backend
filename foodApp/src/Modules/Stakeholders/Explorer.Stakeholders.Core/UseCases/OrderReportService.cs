using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IOrderReportRepository _reportRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderReportService(IOrderReportRepository reportRepository, IOrderRepository orderRepository)
        {
            _reportRepository = reportRepository;
            _orderRepository = orderRepository;
        }

        private static OrderReportDto MapToDto(OrderReport r) => new OrderReportDto
        {
            Id = r.Id,
            OrderId = r.OrderId,
            GuestId = r.GuestId,
            Description = r.Description,
            Answer = r.Answer,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt
        };

        public async Task<OrderReportDto> CreateReportAsync(int guestId, OrderReportDto dto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
                throw new ArgumentException("Order not found.");
            if (order.Status != OrderStatus.Delivered)
                throw new InvalidOperationException("Only delivered orders can be reported.");
            if (order.UserId != guestId)
                throw new InvalidOperationException("You can only report your own orders.");

            var report = new OrderReport(dto.OrderId, guestId, dto.Description!);
            var created = await _reportRepository.CreateAsync(report);
            return MapToDto(created);
        }

        public async Task<List<OrderReportDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            return reports.OrderByDescending(r => r.CreatedAt).Select(MapToDto).ToList();
        }

        public async Task<List<OrderReportDto>> GetGuestReportsAsync(int guestId)
        {
            var reports = await _reportRepository.GetByGuestIdAsync(guestId);
            return reports.OrderByDescending(r => r.CreatedAt).Select(MapToDto).ToList();
        }

        public async Task<OrderReportDto> AnswerReportAsync(int reportId, string answer)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report == null)
                throw new ArgumentException("Report not found.");

            report.SetAnswer(answer);
            var updated = await _reportRepository.UpdateAsync(report);
            return MapToDto(updated);
        }
    }
}
