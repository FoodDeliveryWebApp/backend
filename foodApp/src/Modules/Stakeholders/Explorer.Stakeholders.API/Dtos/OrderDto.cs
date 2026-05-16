using System;
using System.Collections.Generic;

namespace Explorer.Stakeholders.API.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<FoodDto> Foods { get; set; }
        public DateTime OrderTime { get; set; }
        public string Status { get; set; } // Jedan status za sve
        public decimal TotalPrice { get; set; }
        public string Note { get; set; }
    }
}
