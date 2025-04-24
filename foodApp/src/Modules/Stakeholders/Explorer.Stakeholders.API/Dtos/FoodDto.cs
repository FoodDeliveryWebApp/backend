using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class FoodDto
    {
        public long Id { get; set; }  // Id objekta
        public string Name { get; set; }  // Naziv hrane
        public decimal Price { get; set; }  // Cena hrane
        public string Description { get; set; }  // Opis hrane
        public string ImageUrl { get; set; }  // URL slike hrane
        public long RestaurantId { get; set; }  // ID restorana kome hrana pripada
    }
}
