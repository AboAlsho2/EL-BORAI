using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
