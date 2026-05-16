using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class RestaurantApplicationService : IRestaurantApplicationService
    {
        private readonly IRestaurantApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICrudRepository<Person> _personRepository;

        public RestaurantApplicationService(
            IRestaurantApplicationRepository applicationRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository,
            ICrudRepository<Person> personRepository)
        {
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _personRepository = personRepository;
        }

        public async Task<RestaurantApplicationDto> SubmitApplicationAsync(RestaurantApplicationDto dto)
        {
            if (_userRepository.Exists(dto.ManagerUsername))
                throw new ArgumentException("A user with that username already exists.");

            if (!Enum.TryParse<CuisineType>(dto.Cuisine, true, out var cuisine))
                throw new ArgumentException($"Invalid cuisine type: {dto.Cuisine}.");

            var application = new RestaurantApplication(
                dto.RestaurantName, dto.Address, dto.PhoneNumber, cuisine, dto.ImageUrl,
                dto.ManagerUsername, dto.ManagerPassword,
                dto.ManagerName, dto.ManagerSurname, dto.ManagerEmail);

            var created = await _applicationRepository.CreateAsync(application);
            return ToDto(created);
        }

        public async Task<List<RestaurantApplicationDto>> GetAllApplicationsAsync()
        {
            var apps = await _applicationRepository.GetAllAsync();
            return apps.Select(ToDto).ToList();
        }

        public async Task<List<RestaurantApplicationDto>> GetPendingApplicationsAsync()
        {
            var apps = await _applicationRepository.GetByStatusAsync(ApplicationStatus.Pending);
            return apps.Select(ToDto).ToList();
        }

        public async Task<RestaurantApplicationDto> ProcessApplicationAsync(int applicationId, ProcessApplicationDto decision)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                throw new ArgumentException("Application not found.");

            if (string.Equals(decision.Decision, "Approved", StringComparison.OrdinalIgnoreCase))
            {
                var manager = new User(application.ManagerUsername, application.ManagerPassword, UserRole.Manager, true);
                await _userRepository.CreateAsync(manager);

                _personRepository.Create(new Person(manager.Id, application.ManagerName, application.ManagerSurname, application.ManagerEmail));

                var restaurant = new Restaurant(
                    application.RestaurantName, application.Address, application.PhoneNumber,
                    true, application.Cuisine, application.ImageUrl);
                restaurant.SetManager(manager);
                await _restaurantRepository.Create(restaurant);

                application.Approve();
            }
            else if (string.Equals(decision.Decision, "Rejected", StringComparison.OrdinalIgnoreCase))
            {
                application.Reject(decision.AdminComment ?? string.Empty);
            }
            else
            {
                throw new ArgumentException("Decision must be 'Approved' or 'Rejected'.");
            }

            var updated = await _applicationRepository.UpdateAsync(application);
            return ToDto(updated);
        }

        private static RestaurantApplicationDto ToDto(RestaurantApplication a) => new RestaurantApplicationDto
        {
            Id = a.Id,
            RestaurantName = a.RestaurantName,
            Address = a.Address,
            PhoneNumber = a.PhoneNumber,
            Cuisine = a.Cuisine.ToString(),
            ImageUrl = a.ImageUrl,
            ManagerUsername = a.ManagerUsername,
            ManagerPassword = a.ManagerPassword,
            ManagerName = a.ManagerName,
            ManagerSurname = a.ManagerSurname,
            ManagerEmail = a.ManagerEmail,
            Status = a.Status.ToString(),
            CreatedAt = a.CreatedAt,
            AdminComment = a.AdminComment
        };
    }
}
