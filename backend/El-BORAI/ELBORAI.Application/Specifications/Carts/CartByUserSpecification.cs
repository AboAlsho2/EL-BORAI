using ELBORAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Specifications.Carts
{
    public class CartByUserSpecification
        : BaseSpecification<Cart>
    {
        public CartByUserSpecification(int userId)
            : base(c => c.UserId == userId)
        {
        }
    }
}
