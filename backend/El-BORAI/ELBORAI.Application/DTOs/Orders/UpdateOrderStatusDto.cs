using ELBORAI.Domain.Enums;

namespace ELBORAI.Application.DTOs.Orders;

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}