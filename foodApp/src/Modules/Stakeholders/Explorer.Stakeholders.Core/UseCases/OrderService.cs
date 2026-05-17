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
        private readonly IFoodRepository _foodRepository;

        public OrderService(IOrderRepository orderRepository, IRestaurantRepository restaurantRepository, IFoodRepository foodRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _foodRepository = foodRepository;
        }

        private static OrderDto MapToDto(Order order) => new OrderDto
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
            TotalPrice = order.TotalPrice,
            Note = order.Note,
            DeliveryAddress = order.DeliveryAddress,
            PhoneNumber = order.PhoneNumber
        };

        public async Task<(List<OrderDto> Orders, decimal TotalEarnings)> GetAllOrdersAndEarningsForManager(int managerId)
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();

            // Filtriraj restorane koji pripadaju ovom menadžeru
            var managerRestaurants = restaurants.Where(r => r.Manager.Id == managerId).ToList();
            if (!managerRestaurants.Any())
            {
                return (new List<OrderDto>(), 0);
            }

            // Uzmemo sve ID-jeve restorana koje menadžer poseduje
            var restaurantIds = managerRestaurants.Select(r => r.Id).ToList();

            // Filtriraj porudžbine koje pripadaju restoranima ovog menadžera
            var managerOrders = orders.Where(o =>
                o.Foods.Any(f => restaurantIds.Contains(f.RestaurantId))
            ).ToList();

            // Računaj ukupnu zaradu samo od Delivered porudžbina
            decimal totalEarnings = managerOrders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Sum(o => o.Foods.Where(f => restaurantIds.Contains(f.RestaurantId)).Sum(f => f.Price));

            var orderDtos = managerOrders.Select(MapToDto).ToList();

            return (orderDtos, totalEarnings);
        }


        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var foodIds = orderDto.Foods.Select(f => f.Id);
            var foods = await _foodRepository.GetByIdsAsync(foodIds);

            if (foods.Count == 0)
                throw new ArgumentException("None of the requested food items were found.");

            var order = new Order(
                userId: orderDto.UserId,
                foods: foods,
                status: OrderStatus.Pending,
                deliveryAddress: orderDto.DeliveryAddress!,
                phoneNumber: orderDto.PhoneNumber!,
                note: orderDto.Note ?? ""
            );

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            return MapToDto(createdOrder);
        }


        public async Task<List<OrderDto>> GetAllOrdersForWorker(int workerId)
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
                        filteredOrders.Add(MapToDto(order));
                        break;
                    }
                }
            }

            return filteredOrders;
        }

        public async Task<List<OrderDto>> GetAllOrdersForGuest(int guestId)
        {
            // Step 1: Retrieve all orders
            var orders = await _orderRepository.GetAllOrdersAsync(); // Assuming this method exists and fetches all orders.

            // Step 2: Filter orders where the worker is in the restaurant's workers list
            var filteredOrders = new List<OrderDto>();

            foreach (var order in orders)
            {
              
                    if (order.UserId == guestId)
                    {
                        filteredOrders.Add(MapToDto(order));
                    }
                
            }

            return filteredOrders;
        }


        public async Task<OrderDto> UpdateOrderStatus(int orderId, string newStatusString)
        {
            // Step 1: Fetch the order by ID
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }
           
            if (!Enum.TryParse<OrderStatus>(newStatusString, out var newStatus))
            {
                throw new ArgumentException($"Invalid approval status: {newStatusString}");
            }

            // Step 2: Check if the status is valid for this worker
            
            if (order.Status == OrderStatus.Rejected)
            {
                throw new InvalidOperationException("This order has already been rejected and cannot be updated.");
            }

            // Step 3: Update the approval status
            order.Status = newStatus;

            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return MapToDto(updatedOrder);
        }

        public async Task<List<OrderDto>> GetOrdersForDeliveryAsync()
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(OrderStatus.Accepted);
            return orders.Select(MapToDto).ToList();
        }
    }




}






