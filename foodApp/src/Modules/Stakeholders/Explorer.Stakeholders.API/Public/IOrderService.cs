using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Explorer.Stakeholders.API.Public
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
        Task<List<OrderDto>> GetAllOrdersForWorker(long workerId);
        
        Task<List<OrderDto>> GetAllOrdersForGuest(long workerId);
        Task<OrderDto> UpdateOrderStatus(long orderId, string newStatus);
    }
}
