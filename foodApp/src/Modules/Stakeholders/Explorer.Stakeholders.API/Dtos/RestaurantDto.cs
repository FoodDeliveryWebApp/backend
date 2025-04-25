using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RestaurantDto
    {
        public long Id { get; set; }  // Id restorana
        public string Name { get; set; }  // Naziv restorana
        public string Address { get; set; }  // Adresa restorana
        public string PhoneNumber { get; set; }  // Broj telefona restorana
        public bool IsActive { get; set; }  // Status restorana (da li je aktivan)
        public string Cuisine { get; set; }  // Tip kuhinje (npr. Italijanska, Kineska...)
        public string ImageUrl { get; set; }  // URL slike restorana

        public UserDto Manager { get; set; }

    }
}
