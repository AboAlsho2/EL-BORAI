using ELBORAI.Application.DTOs.Carts;

namespace ELBORAI.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartDto?> GetCartAsync();

    Task<CartDto> AddToCartAsync(AddToCartDto dto);

    Task<bool> UpdateQuantityAsync(
        int productId,
        int quantity);

    Task<bool> RemoveFromCartAsync(int productId);

    Task ClearCartAsync();
}