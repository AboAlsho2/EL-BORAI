using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Specifications.Orders;

public class OrderByUserAndIdSpecification
    : BaseSpecification<Order>
{
    public OrderByUserAndIdSpecification(
        int userId,
        int orderId)
        : base(o =>
            o.Id == orderId &&
            o.UserId == userId)
    {
        AddInclude(o => o.Payment);
    }
}