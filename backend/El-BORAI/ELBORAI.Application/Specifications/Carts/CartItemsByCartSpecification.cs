using ELBORAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Specifications.Carts
{
    public class CartItemsByCartSpecification
        : BaseSpecification<CartItem>
    {
        public CartItemsByCartSpecification(int cartId)
            : base(ci => ci.CartId == cartId)
        {
            AddInclude(ci => ci.Product);
        }
    }
}
