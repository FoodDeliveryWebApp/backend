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

        public RestaurantService(IRestaurantRepository restaurantRepository, IUserRepository userRepository)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> AddWorkerToRestaurantAsync(long restaurantId, UserDto workerDto)
        {
            // Fetch the restaurant
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
                return Result.Fail("Restaurant not found.");

            // Create a new worker entity
            var worker = new User(
                workerDto.Username,
                workerDto.Password,
                UserRole.Worker,
                workerDto.IsActive
            );

            // Save the user
            await _userRepository.CreateAsync(worker);

            // Add the worker to the restaurant
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
                foodDto.Name,
                foodDto.Price,
                foodDto.Description,
                foodDto.ImageUrl,
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

            var result = restaurants.Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                PhoneNumber = r.PhoneNumber,
                IsActive = r.IsActive,
                Cuisine = r.GetCuisineTypeName(),
                ImageUrl = r.ImageUrl
            });

            return Result.Ok(result);
        }

        public async Task<RestaurantDto> AddRestaurantAsync(RestaurantDto dto)
        {
            // Create and save the Manager
            var manager = new User(
                dto.Manager.Username,
                dto.Manager.Password,
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

            // Assign the manager
            restaurant.GetType().GetProperty("Manager")?.SetValue(restaurant, manager);

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
