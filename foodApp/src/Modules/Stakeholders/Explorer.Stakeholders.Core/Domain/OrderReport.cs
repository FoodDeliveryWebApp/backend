using Explorer.BuildingBlocks.Core.Domain;
using System;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum OrderReportStatus { Pending, Answered }

    public class OrderReport : Entity
    {
        public int OrderId { get; private set; }
        public int GuestId { get; private set; }
        public string Description { get; private set; }
        public string? Answer { get; private set; }
        public OrderReportStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected OrderReport() { }

        public OrderReport(int orderId, int guestId, string description)
        {
            if (orderId <= 0) throw new ArgumentException("Invalid order ID.");
            if (guestId <= 0) throw new ArgumentException("Invalid guest ID.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.");

            OrderId = orderId;
            GuestId = guestId;
            Description = description.Trim();
            Status = OrderReportStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer)) throw new ArgumentException("Answer cannot be empty.");
            Answer = answer.Trim();
            Status = OrderReportStatus.Answered;
        }
    }
}
