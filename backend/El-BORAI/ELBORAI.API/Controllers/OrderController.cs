using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetMyOrders()
    {
        var orders = await _orderService.GetMyOrdersAsync();

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetMyOrderById(int id)
    {
        var order = await _orderService.GetMyOrderByIdAsync(id);

        if (order is null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await _orderService.CancelOrderAsync(id);

        if (!cancelled)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id:int}/status")]
    [ServiceFilter(typeof(ValidationFilter<UpdateOrderStatusDto>))]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateOrderStatusDto dto)
    {
        var updated = await _orderService.UpdateStatusAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}