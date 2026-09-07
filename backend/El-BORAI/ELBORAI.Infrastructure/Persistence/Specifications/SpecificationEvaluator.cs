using ELBORAI.Application.Specifications;
using ELBORAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator<T>
    where T : BaseEntity
{
    public static IQueryable<T> GetQuery(
        IQueryable<T> inputQuery,
        BaseSpecification<T> specification)
    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        foreach (var include in specification.Includes)
        {
            query = query.Include(include);
        }

        return query;
    }
}