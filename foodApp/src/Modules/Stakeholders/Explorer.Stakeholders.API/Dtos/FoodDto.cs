using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int DeliveryPrice { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int RestaurantId { get; set; }
    }
}
