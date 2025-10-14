using Explorer.BuildingBlocks.Core.Domain;
using System;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum RatingReportStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class RatingReport : Entity
    {
        public Order Order { get; private set; }          // Porudžbina na koju se prijava odnosi
        public User Manager { get; private set; }         // Menadžer koji je obrađuje
        public string Comment { get; private set; }       // Komentar menadžera ili gosta
        public RatingReportStatus Status { get; private set; }  // Status prijave
        public DateTime CreatedAt { get; private set; }   // Kada je prijava kreirana

        // Konstruktor
        public RatingReport(Order order, User manager, string comment, RatingReportStatus status = RatingReportStatus.Pending)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            Comment = comment;
            Status = status;
            CreatedAt = DateTime.UtcNow;

            Validate();
        }

        // Metod za ažuriranje statusa (i eventualnog komentara)
        public void UpdateStatus(RatingReportStatus newStatus, string newComment = "")
        {
            Status = newStatus;

            if (!string.IsNullOrWhiteSpace(newComment))
            {
                Comment = newComment;
            }
        }

        private void Validate()
        {
            if (Comment != null && Comment.Length > 1000)
                throw new ArgumentException("Comment too long (max 1000 characters).");
        }
    }
}
