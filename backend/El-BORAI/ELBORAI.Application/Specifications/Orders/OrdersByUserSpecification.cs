using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Specifications.Orders;

public class OrdersByUserSpecification
    : BaseSpecification<Order>
{
    public OrdersByUserSpecification(int userId)
        : base(o => o.UserId == userId)
    {
        AddInclude(o => o.Payment);
    }
}
