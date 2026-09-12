using ELBORAI.Application.DTOs.Cart;
using ELBORAI.Application.DTOs.Carts;
using ELBORAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        var cart = await _cartService.GetCartAsync();

        if (cart is null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("items")]
    [ServiceFilter(typeof(ValidationFilter<AddToCartDto>))]
    public async Task<ActionResult<CartDto>> AddToCart(AddToCartDto dto)
    {
        var cart = await _cartService.AddToCartAsync(dto);

        return Ok(cart);
    }

    [HttpPut("items/{productId:int}")]
    [ServiceFilter(typeof(ValidationFilter<UpdateCartItemDto>))]
    public async Task<IActionResult> UpdateQuantity(
       int productId,
       UpdateCartItemDto dto)
    {
        var updated = await _cartService
            .UpdateQuantityAsync(productId, dto.Quantity);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var removed = await _cartService
            .RemoveFromCartAsync(productId);

        if (!removed)
            return NotFound();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync();

        return NoContent();
    }
}