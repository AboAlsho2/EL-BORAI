using ELBORAI.Domain.Entities;
using System.Linq.Expressions;

namespace ELBORAI.Application.Specifications;

public class BaseSpecification<T>
    where T : BaseEntity
{
    public Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>> Includes { get; } = new();

    public BaseSpecification(
        Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    protected void AddInclude(
        Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
}