using ELBORAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Specifications.Products
{
    public class AllProductsSpecification
     : BaseSpecification<Product>
    {
        public AllProductsSpecification()
        {
            AddInclude(p => p.Category);
        }
    }
}
