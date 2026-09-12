using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.DTOs.Orders;

public class PaymentDto
{
    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? PaidAt { get; set; }
}