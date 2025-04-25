using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class RestaurantRatingService : IRestaurantRatingService
    {
        private readonly IRestaurantRatingRepository _ratingRepo;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepo;

        public RestaurantRatingService(
            IRestaurantRatingRepository ratingRepo,
            IUserRepository userRepo,
            IRestaurantRepository restaurantRepo)
        {
            _ratingRepo = ratingRepo;
            _userRepository = userRepo;
            _restaurantRepo = restaurantRepo;
        }

        public async Task<Result> AddRatingAsync(RestaurantRatingDto dto)
        {
            // Validate rating value
            if (dto.Rating < 1 || dto.Rating > 10)
                return Result.Fail("Rating must be between 1 and 10.");

            // Validate comment
            if (string.IsNullOrWhiteSpace(dto.Comment))
                return Result.Fail("Comment cannot be empty.");

            // Fetch user
            var user = await _userRepository.GetByIdAsync(dto.RatedByUserId);
            if (user == null)
                return Result.Fail("User not found.");

            // Fetch restaurant
            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
                return Result.Fail("Restaurant not found.");

            // Create domain entity
            var rating = new RestaurantRating(dto.Rating, dto.Comment, user, restaurant);

            // Save
            await _ratingRepo.AddAsync(rating);

            return Result.Ok();
        }
    }
}
