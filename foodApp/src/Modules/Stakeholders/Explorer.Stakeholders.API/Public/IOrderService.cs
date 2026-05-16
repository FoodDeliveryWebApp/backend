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
        
        Task<List<OrderDto>> GetAllOrdersForGuest(int workerId);
        Task<OrderDto> UpdateOrderStatus(int orderId, string newStatus);

        Task<(List<OrderDto> Orders, decimal TotalEarnings)> GetAllOrdersAndEarningsForManager(int managerId);
        Task<List<OrderDto>> GetOrdersForDeliveryAsync();
    }
}
