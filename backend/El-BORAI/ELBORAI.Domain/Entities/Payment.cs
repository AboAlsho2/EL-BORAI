using ELBORAI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Domain.Entities
{


    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        public Order Order { get; set; } = null!;
    }
}

