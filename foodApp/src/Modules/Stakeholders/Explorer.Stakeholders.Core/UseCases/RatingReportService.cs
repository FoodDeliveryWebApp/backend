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
        private readonly IRatingReportRepository _reportRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRestaurantRatingRepository _ratingRepo;
        private readonly IRestaurantRepository _restaurantRepo;

        public RatingReportService(
            IRatingReportRepository reportRepo,
            IUserRepository userRepo,
            IRestaurantRatingRepository ratingRepo,
            IRestaurantRepository restaurantRepo)
        {
            _reportRepo = reportRepo;
            _userRepo = userRepo;
            _ratingRepo = ratingRepo;
            _restaurantRepo = restaurantRepo;
        }

        public async Task<RatingReportDto> CreateReportAsync(int managerId, RatingReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Reason is required.");

            var manager = await _userRepo.GetByIdAsync(managerId)
                ?? throw new ArgumentException("Manager not found.");

            var rating = await _ratingRepo.GetByIdAsync(dto.RatingId)
                ?? throw new ArgumentException("Rating not found.");

            // Ensure rating belongs to a restaurant managed by this manager
            var restaurants = await _restaurantRepo.GetAllRestaurantsAsync();
            var managed = restaurants.FirstOrDefault(r => r.Manager?.Id == managerId);
            if (managed == null || rating.Restaurant.Id != managed.Id)
                throw new InvalidOperationException("You can only report ratings for your own restaurant.");

            if (await _reportRepo.ExistsForRatingAsync(dto.RatingId))
                throw new InvalidOperationException("A pending report already exists for this rating.");

            var report = new RatingReport(rating, manager, dto.Reason);
            var created = await _reportRepo.CreateAsync(report);
            return ToDto(created);
        }

        public async Task<RatingReportDto> UpdateReportStatusAsync(int reportId, string newStatus)
        {
            if (!Enum.TryParse<RatingReportStatus>(newStatus, true, out var status))
                throw new ArgumentException("Invalid status value.");

            var report = await _reportRepo.GetByIdAsync(reportId)
                ?? throw new ArgumentException("Report not found.");

            report.UpdateStatus(status);

            if (status == RatingReportStatus.Approved)
            {
                report.Rating.isDeleted = true;
                await _ratingRepo.UpdateAsync(report.Rating);
            }

            var updated = await _reportRepo.UpdateAsync(report);
            return ToDto(updated);
        }

        public async Task<List<RatingReportDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepo.GetAllAsync();
            return reports.Select(ToDto).ToList();
        }

        public async Task<List<RatingReportDto>> GetManagerReportsAsync(int managerId)
        {
            var reports = await _reportRepo.GetByManagerIdAsync(managerId);
            return reports.Select(ToDto).ToList();
        }

        private static RatingReportDto ToDto(RatingReport r) => new RatingReportDto
        {
            Id = r.Id,
            RatingId = r.Rating.Id,
            ManagerId = r.Manager.Id,
            Reason = r.Reason,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt,
            RatingValue = r.Rating.Rating,
            RatingComment = r.Rating.Comment,
            RestaurantId = r.Rating.Restaurant.Id
        };
    }
}
