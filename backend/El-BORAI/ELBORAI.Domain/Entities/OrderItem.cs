using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
         

         

    }
}
