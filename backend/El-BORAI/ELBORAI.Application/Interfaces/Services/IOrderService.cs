using ELBORAI.Application.DTOs.Orders;

namespace ELBORAI.Application.Interfaces.Services;

public interface IOrderService
{
    Task<IReadOnlyList<OrderDto>> GetMyOrdersAsync();

    Task<OrderDto?> GetMyOrderByIdAsync(int orderId);

    Task<bool> CancelOrderAsync(int orderId);

    Task<bool> UpdateStatusAsync(
        int orderId,
        UpdateOrderStatusDto dto);
}