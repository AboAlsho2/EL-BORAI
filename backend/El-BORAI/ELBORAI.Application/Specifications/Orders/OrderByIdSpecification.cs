using ELBORAI.Application.Specifications;
using ELBORAI.Domain.Entities;

public class OrderByIdSpecification
    : BaseSpecification<Order>
{
    public OrderByIdSpecification(int orderId)
        : base(o => o.Id == orderId)
    {
        AddInclude(o => o.Payment);
    }
}