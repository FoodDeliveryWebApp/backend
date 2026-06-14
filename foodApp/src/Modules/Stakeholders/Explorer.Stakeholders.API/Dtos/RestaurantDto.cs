using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public int DeliveryFee { get; set; }

        public UserDto Manager { get; set; }

    }
}
