using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public OrderService(IOrderRepository orderRepository, IRestaurantRepository restaurantRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;  // Inject the restaurant repository here
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            // Convert DTO to domain model
            var foods = orderDto.Foods.Select(f =>
                new Food(f.Name, f.Price, f.Description, f.ImageUrl, f.RestaurantId)).ToList();

            var order = new Order(
                userId: orderDto.UserId,
                foods: foods,
                status: Enum.Parse<OrderStatus>(orderDto.Status),
                note: orderDto.Note
            );

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Convert domain model back to DTO
            return new OrderDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                Foods = createdOrder.Foods.Select(f => new FoodDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    Description = f.Description,
                    ImageUrl = f.ImageUrl,
                    RestaurantId = f.RestaurantId
                }).ToList(),
                OrderTime = createdOrder.OrderTime,
                Status = createdOrder.Status.ToString(),
                ApprovalStatus = createdOrder.ApprovalStatus.ToString(),
                TotalPrice = createdOrder.TotalPrice,
                Note = createdOrder.Note
            };
        }


        public async Task<List<OrderDto>> GetAllOrdersForWorker(long workerId)
        {
            // Step 1: Retrieve all orders
            var orders = await _orderRepository.GetAllOrdersAsync(); // Assuming this method exists and fetches all orders.

            // Step 2: Filter orders where the worker is in the restaurant's workers list
            var filteredOrders = new List<OrderDto>();

            foreach (var order in orders)
            {
                foreach (var food in order.Foods)
                {
                    var restaurant = await _restaurantRepository.GetRestaurantById(food.RestaurantId); // Await the method to get the restaurant.
                    if (restaurant.Workers.Any(worker => worker.Id == workerId)) // Check if the worker is in the restaurant's worker list.
                    {
                        // Convert order to OrderDto and add it to the filtered list
                        filteredOrders.Add(new OrderDto
                        {
                            Id = order.Id,
                            UserId = order.UserId,
                            Foods = order.Foods.Select(f => new FoodDto
                            {
                                Id = f.Id,
                                Name = f.Name,
                                Price = f.Price,
                                Description = f.Description,
                                ImageUrl = f.ImageUrl,
                                RestaurantId = f.RestaurantId
                            }).ToList(),
                            OrderTime = order.OrderTime,
                            Status = order.Status.ToString(),
                            ApprovalStatus = order.ApprovalStatus.ToString(),
                            TotalPrice = order.TotalPrice,
                            Note = order.Note
                        });
                        break; // No need to check further foods once the worker is found in the restaurant's workers list.
                    }
                }
            }

            return filteredOrders;
        }


        public async Task<OrderDto> UpdateOrderStatus(long orderId, string newStatusString)
        {
            // Step 1: Fetch the order by ID
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }
           
            if (!Enum.TryParse<ApprovalStatus>(newStatusString, out var newStatus))
            {
                throw new ArgumentException($"Invalid approval status: {newStatusString}");
            }

            // Step 2: Check if the status is valid for this worker
            // For example, only a worker can change to "Picked" or "Delivered"
            if (order.ApprovalStatus == ApprovalStatus.Delivered)
            {
                throw new InvalidOperationException("This order has already been delivered and cannot be updated.");
            }

            // Step 3: Update the approval status
            order.ApprovalStatus = newStatus;

            // Step 4: Save the updated order back to the database
            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);

            // Step 5: Convert the domain model to a DTO for the response
            return new OrderDto
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                Foods = updatedOrder.Foods.Select(f => new FoodDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    Description = f.Description,
                    ImageUrl = f.ImageUrl,
                    RestaurantId = f.RestaurantId
                }).ToList(),
                OrderTime = updatedOrder.OrderTime,
                Status = updatedOrder.Status.ToString(),
                ApprovalStatus = updatedOrder.ApprovalStatus.ToString(),
                TotalPrice = updatedOrder.TotalPrice,
                Note = updatedOrder.Note
            };
        }
    }




}






