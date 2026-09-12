using ELBORAI.Application.DTOs.Orders;
using ELBORAI.Application.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Interfaces.Services
{
    public interface ICheckoutService
    {
        Task<OrderDto> CheckoutAsync(CheckoutDto dto);
    }
}
