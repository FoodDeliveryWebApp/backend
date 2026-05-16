using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Explorer.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST api/orders
        [HttpPost]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            var result = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(CreateOrder), new { id = result.Id }, result);
        }


        [HttpGet("worker/{workerId}")]
        [Authorize(Roles = "worker")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersForWorker(int workerId)
        {
            var orders = await _orderService.GetAllOrdersForWorker(workerId);

            if (orders.Count == 0) // No orders for the worker
            {
                return NotFound("No orders found for this worker.");
            }

            return Ok(orders); // Return the list of orders
        }

        [HttpGet("guest/{guestId}")]
        [Authorize(Roles = "guest")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersForGuest(int guestId)
        {
            var orders = await _orderService.GetAllOrdersForGuest(guestId);

            if (orders.Count == 0) // No orders for the worker
            {
                return NotFound("No orders found for this worker.");
            }

            return Ok(orders); // Return the list of orders
        }


        [HttpPut("order/{orderId}/status")]
        [Authorize(Roles = "worker,guest,deliveryman")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            // JWT stores roles in lowercase (administrator, manager, worker, deliveryman, guest)
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.Equals(userRole, "worker", StringComparison.OrdinalIgnoreCase)
                && newStatus != "Accepted" && newStatus != "Rejected")
            {
                return Forbid("Workers can only accept or reject orders.");
            }

            if (string.Equals(userRole, "guest", StringComparison.OrdinalIgnoreCase)
                && newStatus != "Canceled")
            {
                return Forbid("Guests can only cancel their orders.");
            }

            if (string.Equals(userRole, "deliveryman", StringComparison.OrdinalIgnoreCase)
                && newStatus != "InDelivery" && newStatus != "Delivered")
            {
                return Forbid("Delivery persons can only mark orders as InDelivery or Delivered.");
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatus(orderId, newStatus);
                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("delivery")]
        [Authorize(Roles = "deliveryman")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersForDelivery()
        {
            var orders = await _orderService.GetOrdersForDeliveryAsync();
            if (orders.Count == 0)
                return NotFound("No orders available for delivery.");
            return Ok(orders);
        }

        [HttpGet("manager/{managerId}/earnings")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<object>> GetAllOrdersAndEarningsForManager(int managerId)
        {
            var result = await _orderService.GetAllOrdersAndEarningsForManager(managerId);

            if (result.Orders.Count == 0)
            {
                return NotFound("No orders found for this manager's restaurants.");
            }

            return Ok(new
            {
                Orders = result.Orders,
                TotalEarnings = result.TotalEarnings
            });
        }



    }
}
