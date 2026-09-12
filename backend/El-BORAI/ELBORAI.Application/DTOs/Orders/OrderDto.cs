namespace ELBORAI.Application.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderStatus Status { get; set; }

    public PaymentDto Payment { get; set; } = null!;

    public List<OrderItemDto> Items { get; set; } = new();
}