using AbySalto.Junior.DTO;
using AbySalto.Junior.Services.Interfaces;
using FluentValidation;
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
            try
            {
                await _orderService.Create(order, ct);

                return Ok();
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                });

                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal server error."
                });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal server error."
                });
            }
        }

        [HttpPatch]
        [Route("orders/{orderId}/status")]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] UpdateOrderDto updateOrderDto, CancellationToken ct)
        {
            try
            {
                await _orderService.UpdateStatus(orderId, updateOrderDto, ct);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = ex.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = $"Order with id {orderId} not found."
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal server error."
                });
            }
        }

        [HttpGet("orders/{id}/total")]
        public async Task<IActionResult> GetTotal(int id, CancellationToken ct)
        {
            try
            {
                var total = await _orderService.GetOrderTotal(id, ct);
                return Ok(total);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = $"Order with id {id} not found."
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal server error."
                });
            }
        }
    }
}
