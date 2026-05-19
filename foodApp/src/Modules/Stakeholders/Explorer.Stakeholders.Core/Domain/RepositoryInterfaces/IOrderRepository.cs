using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> UpdateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<List<Order>> GetOrdersByDeliveryManAsync(int deliveryManId);
    }
}
