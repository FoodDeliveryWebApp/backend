using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICrudRepository<Person> _personRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository, IUserRepository userRepository, ICrudRepository<Person> personRepository)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<Result> AddWorkerToRestaurantAsync(int restaurantId, UserDto workerDto)
        {
            // Fetch the restaurant
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
                return Result.Fail("Restaurant not found.");

            // Create a new worker entity
            var worker = new User(
                workerDto.Username!,
                workerDto.Password!,
                UserRole.Worker,
                workerDto.IsActive
            );

            await _userRepository.CreateAsync(worker);
            var email = workerDto.Email ?? workerDto.Username;
            _personRepository.Create(new Person(worker.Id, workerDto.Name ?? workerDto.Username!, workerDto.Surname ?? "-", email!));

            restaurant.Workers.Add(worker);
            await _restaurantRepository.UpdateAsync(restaurant);

            return Result.Ok();
        }

        public async Task<Result> AddFoodToRestaurantAsync(FoodDto foodDto)
        {
            // Fetch the restaurant
            var restaurant = await _restaurantRepository.GetByIdAsync(foodDto.RestaurantId);
            if (restaurant == null)
                return Result.Fail("Restaurant not found.");

            // Create a new food item
            var food = new Food(
                foodDto.Name!,
                foodDto.Price,
                foodDto.Description!,
                foodDto.ImageUrl!,
                foodDto.RestaurantId
            );

            // Add the food to the restaurant
            restaurant.Foods.Add(food);
            await _restaurantRepository.UpdateAsync(restaurant);

            return Result.Ok();
        }




        public async Task<Result<IEnumerable<RestaurantDto>>> GetAllRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
            if (restaurants == null || !restaurants.Any())
                return Result.Fail("No restaurants found.");

            var result = restaurants.Select(r =>
            {
                UserDto? managerDto = null;
                if (r.Manager != null)
                {
                    var person = _userRepository.GetPersonByUserId(r.Manager.Id);
                    managerDto = new UserDto
                    {
                        Id = r.Manager.Id,
                        Username = r.Manager.Username,
                        Role = r.Manager.Role.ToString(),
                        IsActive = r.Manager.IsActive,
                        Name = person?.Name,
                        Surname = person?.Surname,
                        Email = person?.Email
                    };
                }
                return new RestaurantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    IsActive = r.IsActive,
                    Cuisine = r.GetCuisineTypeName(),
                    ImageUrl = r.ImageUrl,
                    Manager = managerDto
                };
            });

            return Result.Ok(result);
        }

        public async Task<Result<IEnumerable<UserDto>>> GetWorkersAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null) return Result.Fail("Restaurant not found.");

            var workers = restaurant.Workers.Select(w =>
            {
                var person = _userRepository.GetPersonByUserId(w.Id);
                return new UserDto
                {
                    Id = w.Id,
                    Username = w.Username,
                    IsActive = w.IsActive,
                    Role = w.Role.ToString(),
                    Name = person?.Name,
                    Surname = person?.Surname,
                    Email = person?.Email
                };
            });

            return Result.Ok(workers);
        }

        public async Task<Result> RemoveWorkerAsync(int restaurantId, int workerId)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null) return Result.Fail("Restaurant not found.");

            var worker = restaurant.Workers.FirstOrDefault(w => w.Id == workerId);
            if (worker == null) return Result.Fail("Worker not found in this restaurant.");

            restaurant.Workers.Remove(worker);
            await _restaurantRepository.UpdateAsync(restaurant);

            return Result.Ok();
        }

        public async Task<Result> UpdateWorkerAsync(int restaurantId, int workerId, UserDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null) return Result.Fail("Restaurant not found.");

            var worker = restaurant.Workers.FirstOrDefault(w => w.Id == workerId);
            if (worker == null) return Result.Fail("Worker not found in this restaurant.");

            worker.IsActive = dto.IsActive;
            await _userRepository.UpdateAsync(worker);

            var person = _userRepository.GetPersonByUserId(workerId);
            if (person != null)
            {
                if (dto.Name != null) person.Name = dto.Name;
                if (dto.Surname != null) person.Surname = dto.Surname;
                if (dto.Email != null) person.Email = dto.Email;
                _personRepository.Update(person);
            }

            return Result.Ok();
        }

        public async Task<Result<RestaurantDto>> UpdateRestaurantAsync(int id, RestaurantDto dto)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return Result.Fail("Restaurant not found.");

            if (!Enum.TryParse<CuisineType>(dto.Cuisine, true, out var cuisineEnum))
                return Result.Fail("Invalid cuisine type.");

            restaurant.Update(dto.Name, dto.Address, dto.PhoneNumber, dto.IsActive, cuisineEnum, dto.ImageUrl);

            if (dto.Manager != null && !string.IsNullOrWhiteSpace(dto.Manager.Password))
            {
                // Create new manager user and assign to restaurant
                if (_userRepository.Exists(dto.Manager.Username!))
                    return Result.Fail("Manager username already taken.");

                var newManager = new User(dto.Manager.Username!, dto.Manager.Password, UserRole.Manager, true);
                await _userRepository.CreateAsync(newManager);
                _personRepository.Create(new Person(newManager.Id, dto.Manager.Name ?? dto.Manager.Username!, dto.Manager.Surname ?? "-", dto.Manager.Email ?? dto.Manager.Username!));
                restaurant.SetManager(newManager);
            }
            else if (dto.Manager != null && restaurant.Manager != null)
            {
                // Update existing manager's person info only
                var existingManager = restaurant.Manager;
                existingManager.IsActive = dto.Manager.IsActive;
                await _userRepository.UpdateAsync(existingManager);

                var person = _userRepository.GetPersonByUserId(existingManager.Id);
                if (person != null)
                {
                    if (dto.Manager.Name != null) person.Name = dto.Manager.Name;
                    if (dto.Manager.Surname != null) person.Surname = dto.Manager.Surname;
                    if (dto.Manager.Email != null) person.Email = dto.Manager.Email;
                    _personRepository.Update(person);
                }
            }

            await _restaurantRepository.UpdateAsync(restaurant);

            var managerPerson = restaurant.Manager != null ? _userRepository.GetPersonByUserId(restaurant.Manager.Id) : null;
            return Result.Ok(new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                IsActive = restaurant.IsActive,
                Cuisine = restaurant.GetCuisineTypeName(),
                ImageUrl = restaurant.ImageUrl,
                Manager = restaurant.Manager == null ? null : new UserDto
                {
                    Id = restaurant.Manager.Id,
                    Username = restaurant.Manager.Username,
                    Role = restaurant.Manager.Role.ToString(),
                    IsActive = restaurant.Manager.IsActive,
                    Name = managerPerson?.Name,
                    Surname = managerPerson?.Surname,
                    Email = managerPerson?.Email
                }
            });
        }

        public async Task<Result> DeleteRestaurantAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return Result.Fail("Restaurant not found.");

            if (restaurant.Manager != null)
            {
                var managerId = restaurant.Manager.Id;
                var person = _userRepository.GetPersonByUserId(managerId);
                if (person != null) _personRepository.Delete(person.Id);
                await _userRepository.DeleteAsync(managerId);
            }

            await _restaurantRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<RestaurantDto> AddRestaurantAsync(RestaurantDto dto)
        {
            // Create and save the Manager
            var manager = new User(
                dto.Manager.Username!,
                dto.Manager.Password!,
                UserRole.Manager,
                dto.Manager.IsActive
            );

            await _userRepository.CreateAsync(manager);

            // Parse cuisine enum from string
            if (!Enum.TryParse<CuisineType>(dto.Cuisine, true, out var cuisineEnum))
            {
                throw new ArgumentException("Invalid cuisine type.");
            }

            // Create and save the restaurant
            var restaurant = new Restaurant(
                dto.Name,
                dto.Address,
                dto.PhoneNumber,
                dto.IsActive,
                cuisineEnum,
                dto.ImageUrl
            );

            restaurant.SetManager(manager);
            await _restaurantRepository.Create(restaurant);

            // Return the DTO
            return new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                IsActive = restaurant.IsActive,
                Cuisine = restaurant.GetCuisineTypeName(),
                ImageUrl = restaurant.ImageUrl,
                Manager = new UserDto
                {
                    Username = manager.Username,
                    Password = manager.Password,
                    Role = manager.Role.ToString(),
                    IsActive = manager.IsActive
                }
            };
        }



    }




}
