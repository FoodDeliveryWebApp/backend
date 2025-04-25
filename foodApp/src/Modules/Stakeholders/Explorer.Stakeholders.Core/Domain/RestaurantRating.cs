using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class RestaurantRating : Entity
    {
        public int Rating { get; private set; } // Rating from 1 to 10
        public string Comment { get; private set; }
        public User RatedBy { get; private set; }
        public Restaurant Restaurant { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public RestaurantRating(int rating, string comment, User ratedBy, Restaurant restaurant)
        {
            Rating = rating;
            Comment = comment;
            RatedBy = ratedBy ?? throw new ArgumentNullException(nameof(ratedBy));
            Restaurant = restaurant ?? throw new ArgumentNullException(nameof(restaurant));
            CreatedAt = DateTime.UtcNow;

            Validate();
        }

        private void Validate()
        {
            if (Rating < 1 || Rating > 10)
                throw new ArgumentException("Rating must be between 1 and 10.");
            if (string.IsNullOrWhiteSpace(Comment))
                throw new ArgumentException("Comment cannot be empty.");
        }
    }
}
