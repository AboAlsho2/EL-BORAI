using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }

        public Payment Payment { get; set; }

       // public Stauts OrderStatus { get; set; }

    }
}
