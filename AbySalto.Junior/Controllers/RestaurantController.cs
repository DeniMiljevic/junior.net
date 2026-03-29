using AbySalto.Junior.DTO;
using AbySalto.Junior.Enums;
using AbySalto.Junior.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Junior.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public RestaurantController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("orders")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto order, CancellationToken ct)
        {
            if (order == null) return BadRequest("Order data is required.");
            try
            {
                await _orderService.Create(order, ct);
                return Ok("Order created successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> GetAll([FromQuery] bool orderByTotal, [FromQuery] bool descending, CancellationToken ct)
        {
            try
            {
                var orders = await _orderService.GetAll(orderByTotal, descending, ct);
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpPatch]
        [Route("orders/{orderId}/status")]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] UpdateOrderDto updateOrderDto, CancellationToken ct)
        {
            try
            {
                await _orderService.Update(orderId, updateOrderDto, ct);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpGet("orders/{id}/total")]
        public async Task<IActionResult> GetTotal(int id, CancellationToken ct)
        {
            var total = await _orderService.GetOrderTotalAsync(id, ct);
            return Ok(new { total });
        }
    }
}
