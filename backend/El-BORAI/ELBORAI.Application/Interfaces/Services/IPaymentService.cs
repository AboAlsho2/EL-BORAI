using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentDto?> GetByOrderIdAsync(int orderId);

    Task<bool> UpdateStatusAsync(
        int orderId,
        PaymentStatus status);
}