using ELBORAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Specifications.Carts
{
    public class CartItemByCartAndProductSpecification
      : BaseSpecification<CartItem>
    {
        public CartItemByCartAndProductSpecification(
            int cartId,
            int productId)
            : base(ci =>
                ci.CartId == cartId &&
                ci.ProductId == productId)
        {
        }
    }
}
