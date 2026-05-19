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
        Pending,     // Newly placed by guest, awaiting worker action
        Accepted,    // Accepted by worker, being prepared
        Rejected,    // Rejected by worker
        Preparing,   // Being prepared (legacy / alternative to Accepted)
        ToPickUp,    // Claimed by a delivery man, will pick up soon
        InDelivery,  // Picked up by delivery person
        Delivered,   // Successfully delivered
        Canceled,    // Canceled by guest
        PickUp,      // Ready for guest pickup
        Delivery     // Delivery type marker (kept for compatibility)
    }

   

    public class Order : Entity
    {
        public int UserId { get; private set; }
        public List<Food> Foods { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice => Foods.Sum(f => f.Price);
        public int DeliveryPrice => Foods.Sum(f => f.DeliveryPrice);
        public int? DeliveryManId { get; private set; }
        public string Note { get; private set; }
        public string DeliveryAddress { get; private set; }
        public string PhoneNumber { get; private set; }

        protected Order() { }

        public Order(int userId, List<Food> foods, OrderStatus status, string deliveryAddress, string phoneNumber, string note = "")
        {
            UserId = userId;
            Foods = foods ?? throw new ArgumentNullException(nameof(foods));
            Status = status;
            OrderTime = DateTime.UtcNow;
            Note = note;
            DeliveryAddress = deliveryAddress;
            PhoneNumber = phoneNumber;

            Validate();
        }

        public void AssignDeliveryMan(int deliveryManId)
        {
            DeliveryManId = deliveryManId;
        }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid user ID.");
            if (Foods == null || Foods.Count == 0) throw new ArgumentException("Order must contain at least one food item.");
            if (Note.Length > 500) throw new ArgumentException("Note is too long (max 500 characters).");
            if (string.IsNullOrWhiteSpace(DeliveryAddress)) throw new ArgumentException("Delivery address is required.");
            if (string.IsNullOrWhiteSpace(PhoneNumber)) throw new ArgumentException("Phone number is required.");
        }
    }
}
