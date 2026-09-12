using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Specifications.Payments;

public class PaymentByOrderSpecification
    : BaseSpecification<Payment>
{
    public PaymentByOrderSpecification(int orderId)
        : base(p => p.OrderId == orderId)
    {
        AddInclude(p => p.Order);
    }
}