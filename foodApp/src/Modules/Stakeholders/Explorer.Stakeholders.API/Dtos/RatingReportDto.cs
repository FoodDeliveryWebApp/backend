using System;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RatingReportDto
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public int ManagerId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        // Populated on read — display info for admin
        public int RatingValue { get; set; }
        public string? RatingComment { get; set; }
        public int RestaurantId { get; set; }
    }
}
