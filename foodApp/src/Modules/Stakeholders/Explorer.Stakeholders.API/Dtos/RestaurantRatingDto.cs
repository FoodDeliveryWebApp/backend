using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RestaurantRatingDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int RatedByUserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
