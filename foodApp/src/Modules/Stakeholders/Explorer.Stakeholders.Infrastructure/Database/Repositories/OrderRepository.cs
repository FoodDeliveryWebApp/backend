using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StakeholdersContext _context;

        public OrderRepository(StakeholdersContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Add the new order to the context
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            // Return all orders from the database
            return await _context.Orders
                .Include(o => o.Foods)  // Assuming 'Foods' is a navigation property
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            // Find order by ID, including its related entities
            return await _context.Orders
                .Include(o => o.Foods)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Include(o => o.Foods)
                .Where(o => o.Status == status)
                .ToListAsync();
        }
    }
}