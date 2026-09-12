using ELBORAI.Application.DTOs.Payment;
using ELBORAI.Domain.Enums;
using FluentValidation;

namespace ELBORAI.Application.Validators.Payment;

public class CheckoutDtoValidator
    : AbstractValidator<CheckoutDto>
{
    public CheckoutDtoValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .IsInEnum();
    }
}