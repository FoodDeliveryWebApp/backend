namespace Explorer.Stakeholders.API.Dtos
{
    public class RestaurantApplicationDto
    {
        public long Id { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Cuisine { get; set; }
        public string ImageUrl { get; set; }
        public string ManagerUsername { get; set; }
        public string ManagerPassword { get; set; }
        public string ManagerName { get; set; }
        public string ManagerSurname { get; set; }
        public string ManagerEmail { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AdminComment { get; set; }
    }

    public class ProcessApplicationDto
    {
        public string Decision { get; set; }   // "Approved" or "Rejected"
        public string AdminComment { get; set; }
    }
}
