using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Explorer.Stakeholders.API.Public
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
        Task<List<OrderDto>> GetAllOrdersForWorker(int workerId);
        Task<List<OrderDto>> GetAllOrdersForGuest(int guestId);
        Task<OrderDto> UpdateOrderStatus(int orderId, string newStatus, int? actorId = null);
        Task<(List<OrderDto> Orders, decimal TotalEarnings)> GetAllOrdersAndEarningsForManager(int managerId);
        Task<List<OrderDto>> GetOrdersForDeliveryAsync();
        Task<(List<OrderDto> Orders, int TotalEarnings)> GetOrdersByDeliveryManAsync(int deliveryManId);
    }
}
