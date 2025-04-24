using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public List<FoodDto> Foods { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; } // "PickUp" or "Delivery"
        public string ApprovalStatus { get; set; } // "Pending", "Approved", or "Rejected"
        public decimal TotalPrice { get; set; }
        public string Note { get; set; }
    }

}
