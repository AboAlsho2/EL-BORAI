using ELBORAI.Application.Specifications;
using ELBORAI.Domain.Entities;

public class OrderItemsByOrderSpecification
    : BaseSpecification<OrderItem>
{
    public OrderItemsByOrderSpecification(int orderId)
        : base(oi => oi.OrderId == orderId)
    {
        AddInclude(oi => oi.Product);
    }
}