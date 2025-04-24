using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Food : Entity
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }

        public long RestaurantId { get; private set; }  

        public Food(string name, decimal price, string description, string imageUrl, long restaurantId)
        {
            Name = name;
            Price = price;
            Description = description;
            ImageUrl = imageUrl;
            RestaurantId = restaurantId;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid food name.");
            if (Price <= 0) throw new ArgumentException("Invalid food price.");
            if (string.IsNullOrWhiteSpace(ImageUrl)) throw new ArgumentException("Invalid image URL.");
        }


    }
}
