using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public enum ApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class RestaurantApplication : Entity
    {
        public string RestaurantName { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public CuisineType Cuisine { get; private set; }
        public string ImageUrl { get; private set; }

        public string ManagerUsername { get; private set; }
        public string ManagerPassword { get; private set; }
        public string ManagerName { get; private set; }
        public string ManagerSurname { get; private set; }
        public string ManagerEmail { get; private set; }

        public ApplicationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string AdminComment { get; private set; }

        protected RestaurantApplication() { }

        public RestaurantApplication(
            string restaurantName, string address, string phoneNumber,
            CuisineType cuisine, string imageUrl,
            string managerUsername, string managerPassword,
            string managerName, string managerSurname, string managerEmail)
        {
            RestaurantName = restaurantName;
            Address = address;
            PhoneNumber = phoneNumber;
            Cuisine = cuisine;
            ImageUrl = imageUrl;
            ManagerUsername = managerUsername;
            ManagerPassword = managerPassword;
            ManagerName = managerName;
            ManagerSurname = managerSurname;
            ManagerEmail = managerEmail;
            Status = ApplicationStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            AdminComment = string.Empty;
            Validate();
        }

        public void Approve()
        {
            if (Status != ApplicationStatus.Pending)
                throw new InvalidOperationException("Only pending applications can be approved.");
            Status = ApplicationStatus.Approved;
        }

        public void Reject(string comment)
        {
            if (Status != ApplicationStatus.Pending)
                throw new InvalidOperationException("Only pending applications can be rejected.");
            Status = ApplicationStatus.Rejected;
            AdminComment = comment ?? string.Empty;
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(RestaurantName)) throw new ArgumentException("Restaurant name is required.");
            if (string.IsNullOrWhiteSpace(Address)) throw new ArgumentException("Address is required.");
            if (string.IsNullOrWhiteSpace(PhoneNumber)) throw new ArgumentException("Phone number is required.");
            if (string.IsNullOrWhiteSpace(ImageUrl)) throw new ArgumentException("Image URL is required.");
            if (string.IsNullOrWhiteSpace(ManagerUsername)) throw new ArgumentException("Manager username is required.");
            if (string.IsNullOrWhiteSpace(ManagerPassword)) throw new ArgumentException("Manager password is required.");
            if (string.IsNullOrWhiteSpace(ManagerName)) throw new ArgumentException("Manager name is required.");
            if (string.IsNullOrWhiteSpace(ManagerSurname)) throw new ArgumentException("Manager surname is required.");
            if (string.IsNullOrWhiteSpace(ManagerEmail)) throw new ArgumentException("Manager email is required.");
        }
    }
}
