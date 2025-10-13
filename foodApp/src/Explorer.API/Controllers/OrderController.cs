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
        [Authorize(Roles = "Guest")]  // Only allow Guest users to access this endpoint
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            // Check if the User has the 'Guest' role
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != "Guest")
            {
                return Unauthorized("Only Guests are allowed to place orders.");
            }

            var result = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(CreateOrder), new { id = result.Id }, result);
        }


        [HttpGet("worker/{workerId}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersForWorker(long workerId)
        {
            var orders = await _orderService.GetAllOrdersForWorker(workerId);

            if (orders.Count == 0) // No orders for the worker
            {
                return NotFound("No orders found for this worker.");
            }

            return Ok(orders); // Return the list of orders
        }

        [HttpGet("guest/{guestId}")]
        [Authorize(Roles = "Guest")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersForGuest(long guestId)
        {
            var orders = await _orderService.GetAllOrdersForGuest(guestId);

            if (orders.Count == 0) // No orders for the worker
            {
                return NotFound("No orders found for this worker.");
            }

            return Ok(orders); // Return the list of orders
        }


        [HttpPut("order/{orderId}/status")]
        [Authorize(Roles = "Worker,Guest,DeliveryMan")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(long orderId, [FromBody] string newStatus)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);

          
            if (userRole == "Guest" && newStatus != "Canceled")
            {
                return Forbid("Guests can only cancel their orders.");
            }

            if (userRole == "DeliveryMan" && newStatus != "Delivered" && newStatus != "InDelivery")
            {
                return Forbid("DeliveryMen can only mark orders as InDelivery or Delivered.");
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatus(orderId, newStatus);
                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);  // Ako porudžbina nije pronađena
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);  // Ako status ne može da se promeni
            }
        }





    }
}
