using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Restaurant : Entity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsActive { get; set; }
        public CuisineType Cuisine { get; private set; }
        public string ImageUrl { get; private set; }
        public List<Food> Foods { get; private set; } = new();
        public List<User> Workers { get; private set; } = new();

        public User Manager { get; private set; }

        public Restaurant(string name, string address, string phoneNumber, bool isActive, CuisineType cuisine, string imageUrl)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            IsActive = isActive;
            Cuisine = cuisine;
            ImageUrl = imageUrl;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid restaurant name.");
            if (string.IsNullOrWhiteSpace(Address)) throw new ArgumentException("Invalid restaurant address.");
            if (string.IsNullOrWhiteSpace(PhoneNumber)) throw new ArgumentException("Invalid phone number.");
            if (string.IsNullOrWhiteSpace(ImageUrl)) throw new ArgumentException("Invalid image URL.");
        }

        public string GetCuisineTypeName()
        {
            return Cuisine.ToString().ToLower();
        }
    }

    public enum CuisineType
    {
        Italian,
        Chinese,
        Serbian,
        Indian,
        Mexican,
        American,
        Other
    }
}
