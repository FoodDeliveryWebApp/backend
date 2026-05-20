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
            Foods = order.Items.SelectMany(i => Enumerable.Repeat(new FoodDto
            {
                Id = i.Food.Id,
                Name = i.Food.Name,
                Price = i.Food.Price,
                DeliveryPrice = i.Food.DeliveryPrice,
                Description = i.Food.Description,
                ImageUrl = i.Food.ImageUrl,
                RestaurantId = i.Food.RestaurantId
            }, i.Quantity)).ToList(),
            OrderTime = order.OrderTime,
            Status = order.Status.ToString(),
            TotalPrice = order.TotalPrice,
            DeliveryPrice = order.DeliveryPrice,
            DeliveryManId = order.DeliveryManId,
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
                o.Items.Any(i => restaurantIds.Contains(i.Food.RestaurantId))
            ).ToList();

            // Računaj ukupnu zaradu samo od Delivered porudžbina
            decimal totalEarnings = managerOrders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Sum(o => o.Items.Where(i => restaurantIds.Contains(i.Food.RestaurantId)).Sum(i => i.Food.Price * i.Quantity));

            var orderDtos = managerOrders.Select(MapToDto).ToList();

            return (orderDtos, totalEarnings);
        }


        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var distinctIds = orderDto.Foods.Select(f => f.Id).Distinct();
            var foodEntities = await _foodRepository.GetByIdsAsync(distinctIds);

            if (foodEntities.Count == 0)
                throw new ArgumentException("None of the requested food items were found.");

            var foodMap = foodEntities.ToDictionary(f => f.Id);
            var items = orderDto.Foods
                .GroupBy(f => f.Id)
                .Where(g => foodMap.ContainsKey(g.Key))
                .Select(g => new OrderItem(g.Key, g.Count(), foodMap[g.Key]))
                .ToList();

            var order = new Order(
                userId: orderDto.UserId,
                items: items,
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
                foreach (var item in order.Items)
                {
                    var restaurant = await _restaurantRepository.GetRestaurantById(item.Food.RestaurantId);
                    if (restaurant.Workers.Any(worker => worker.Id == workerId))
                    {
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


        public async Task<OrderDto> UpdateOrderStatus(int orderId, string newStatusString, int? actorId = null)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            if (!Enum.TryParse<OrderStatus>(newStatusString, out var newStatus))
                throw new ArgumentException($"Invalid status: {newStatusString}");

            if (order.Status == OrderStatus.Rejected)
                throw new InvalidOperationException("This order has already been rejected and cannot be updated.");

            if (newStatus == OrderStatus.ToPickUp)
            {
                if (actorId == null)
                    throw new ArgumentException("Delivery man ID required when claiming an order.");
                order.AssignDeliveryMan(actorId.Value);
            }

            order.Status = newStatus;

            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return MapToDto(updatedOrder);
        }

        public async Task<List<OrderDto>> GetOrdersForDeliveryAsync()
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(OrderStatus.Accepted);
            return orders.Where(o => o.DeliveryManId == null).Select(MapToDto).ToList();
        }

        public async Task<(List<OrderDto> Orders, int TotalEarnings)> GetOrdersByDeliveryManAsync(int deliveryManId)
        {
            var orders = await _orderRepository.GetOrdersByDeliveryManAsync(deliveryManId);
            var totalEarnings = orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Sum(o => o.DeliveryPrice);
            return (orders.Select(MapToDto).ToList(), totalEarnings);
        }
    }




}






