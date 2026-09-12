using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("order/{orderId:int}")]
    public async Task<ActionResult<PaymentDto>> GetByOrderId(int orderId)
    {
        var payment = await _paymentService.GetByOrderIdAsync(orderId);

        if (payment is null)
            return NotFound();

        return Ok(payment);
    }

    [HttpPut("order/{orderId:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int orderId,
        [FromQuery] PaymentStatus status)
    {
        var updated = await _paymentService
            .UpdateStatusAsync(orderId, status);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}