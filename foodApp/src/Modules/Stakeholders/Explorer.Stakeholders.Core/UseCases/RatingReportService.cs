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
    public class RatingReportService : IRatingReportService
    {
        private readonly IRatingReportRepository _ratingReportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public RatingReportService(
            IRatingReportRepository ratingReportRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository)
        {
            _ratingReportRepository = ratingReportRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<RatingReportDto> CreateReportAsync(RatingReportDto dto)
        {
            var manager = await _userRepository.GetByIdAsync(dto.ManagerId);
            var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);

            if (manager == null) throw new ArgumentException("Manager not found.");
            if (order == null) throw new ArgumentException("Order not found.");

            var statusEnum = Enum.Parse<RatingReportStatus>(dto.Status, true);

            var report = new RatingReport(order, manager, dto.Comment, statusEnum);

            var created = await _ratingReportRepository.CreateAsync(report);

            return ToDto(created);
        }

        public async Task<RatingReportDto> UpdateReportStatusAsync(long reportId, string newStatus)
        {
            var report = await _ratingReportRepository.GetByIdAsync(reportId);
            if (report == null)
                throw new ArgumentException("Report not found.");

            if (!Enum.TryParse<RatingReportStatus>(newStatus, true, out var parsedStatus))
                throw new ArgumentException("Invalid status value.");

            report.UpdateStatus(parsedStatus);
            var updated = await _ratingReportRepository.UpdateAsync(report);

            return ToDto(updated);
        }

        public async Task<List<RatingReportDto>> GetAllReportsAsync()
        {
            var reports = await _ratingReportRepository.GetAllAsync();
            return reports.Select(ToDto).ToList();
        }

        private static RatingReportDto ToDto(RatingReport report)
        {
            return new RatingReportDto
            {
                Id = report.Id,
                OrderId = report.Order.Id,
                ManagerId = report.Manager.Id,
                Comment = report.Comment,
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }
    }
}
