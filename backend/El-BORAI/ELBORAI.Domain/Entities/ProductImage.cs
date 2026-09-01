using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{
    public class ProductImage :BaseEntity
    {
        public string ImageUrl { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
