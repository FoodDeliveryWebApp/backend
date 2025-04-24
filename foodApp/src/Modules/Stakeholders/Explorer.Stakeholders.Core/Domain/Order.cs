using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum OrderStatus
    {
        PickUp,
        Delivery
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected,
        Picked,
        Delivered
    }

    public class Order : Entity
    {
        public long UserId { get; private set; }
        public List<Food> Foods { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderStatus Status { get; private set; }
        public ApprovalStatus ApprovalStatus { get;  set; }
        public decimal TotalPrice => Foods.Sum(f => f.Price);
        public string Note { get; private set; }

        public Order(long userId, List<Food> foods, OrderStatus status, string note = "")
        {
            UserId = userId;
            Foods = foods ?? throw new ArgumentNullException(nameof(foods));
            Status = status;
            ApprovalStatus = ApprovalStatus.Pending;
            OrderTime = DateTime.UtcNow;
            Note = note;

            Validate();
        }

        private void Validate()
        {
            if (UserId <= 0) throw new ArgumentException("Invalid user ID.");
            if (Foods == null || Foods.Count == 0) throw new ArgumentException("Order must contain at least one food item.");
            if (Note.Length > 500) throw new ArgumentException("Note is too long (max 500 characters).");
        }

        public void Approve()
        {
            ApprovalStatus = ApprovalStatus.Approved;
        }

        public void Reject()
        {
            ApprovalStatus = ApprovalStatus.Rejected;
        }
    }
}
