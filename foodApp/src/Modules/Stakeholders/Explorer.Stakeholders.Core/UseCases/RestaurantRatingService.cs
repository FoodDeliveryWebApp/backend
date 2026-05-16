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
        private readonly IOrderRepository _orderRepository;

        public RestaurantRatingService(
            IRestaurantRatingRepository ratingRepo,
            IUserRepository userRepo,
            IRestaurantRepository restaurantRepo,
            IOrderRepository orderRepository)
        {
            _ratingRepo = ratingRepo;
            _userRepository = userRepo;
            _restaurantRepo = restaurantRepo;
            _orderRepository = orderRepository;
        }

        public async Task<Result> AddRatingAsync(RestaurantRatingDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 10)
                return Result.Fail("Rating must be between 1 and 10.");

            if (string.IsNullOrWhiteSpace(dto.Comment))
                return Result.Fail("Comment cannot be empty.");

            var user = await _userRepository.GetByIdAsync(dto.RatedByUserId);
            if (user == null)
                return Result.Fail("User not found.");

            var restaurant = await _restaurantRepo.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
                return Result.Fail("Restaurant not found.");

            // Guest must have at least one delivered order from this restaurant
            var guestOrders = await _orderRepository.GetOrdersByStatusAsync(OrderStatus.Delivered);
            var hasOrderedFromRestaurant = guestOrders.Any(o =>
                o.UserId == dto.RatedByUserId &&
                o.Foods.Any(f => f.RestaurantId == dto.RestaurantId));

            if (!hasOrderedFromRestaurant)
                return Result.Fail("You can only rate a restaurant after a delivered order from it.");

            var rating = new RestaurantRating(dto.Rating, dto.Comment, user, restaurant);
            await _ratingRepo.AddAsync(rating);

            return Result.Ok();
        }

        public async Task<List<RestaurantRatingDto>> GetRatingsForRestaurantAsync(int restaurantId)
        {
            // Fetch all ratings for the restaurant
            var ratings = await _ratingRepo.GetByRestaurantIdAsync(restaurantId);

            // Map domain entities to DTOs
            var ratingDtos = ratings.Select(r => new RestaurantRatingDto
            {
                Rating = r.Rating,
                Comment = r.Comment,
                RatedByUserId = r.RatedBy.Id,
                RestaurantId = r.Restaurant.Id,
                CreatedAt = r.CreatedAt
            }).ToList();

            return ratingDtos;
        }
    }
}