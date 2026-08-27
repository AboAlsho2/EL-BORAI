using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }

        public int Quantity { get; set; }
    }
}
