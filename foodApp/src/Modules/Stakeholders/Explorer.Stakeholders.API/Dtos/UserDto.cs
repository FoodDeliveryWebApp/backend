using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }     // Očekuje se "Manager"
        public bool IsActive { get; set; }
    }
}
