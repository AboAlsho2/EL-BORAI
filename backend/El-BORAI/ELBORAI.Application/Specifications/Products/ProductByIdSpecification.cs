using ELBORAI.Domain.Entities;

namespace ELBORAI.Application.Specifications.Products;

public class ProductByIdSpecification
    : BaseSpecification<Product>
{
    public ProductByIdSpecification(int id)
        : base(p => p.Id == id)
    {
        AddInclude(p => p.Category);
        
    }
}