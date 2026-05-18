using Explorer.BuildingBlocks.Core.Domain;
using System;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum RatingReportStatus { Pending, Approved, Rejected }

    public class RatingReport : Entity
    {
        public RestaurantRating Rating { get; private set; }
        public User Manager { get; private set; }
        public string Reason { get; private set; }
        public RatingReportStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected RatingReport() { }

        public RatingReport(RestaurantRating rating, User manager, string reason)
        {
            Rating = rating ?? throw new ArgumentNullException(nameof(rating));
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason is required.");
            Reason = reason.Trim();
            Status = RatingReportStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(RatingReportStatus newStatus) => Status = newStatus;
    }
}
