using ELBORAI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.DTOs.Payment
{
    public class CheckoutDto
    {
        public PaymentMethod PaymentMethod { get; set; }
    }
}
