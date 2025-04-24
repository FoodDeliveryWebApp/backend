using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    internal class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
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
    }
}
