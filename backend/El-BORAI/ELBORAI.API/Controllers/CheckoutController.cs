using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CheckoutDto>))]
    public async Task<ActionResult<OrderDto>> Checkout(CheckoutDto dto)
    {
        var order = await _checkoutService.CheckoutAsync(dto);

        return Ok(order);
    }
}