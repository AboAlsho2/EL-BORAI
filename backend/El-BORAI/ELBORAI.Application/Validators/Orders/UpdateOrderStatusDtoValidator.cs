using ELBORAI.Application.DTOs.Orders;
using FluentValidation;

namespace ELBORAI.Application.Validators.Orders;

public class UpdateOrderStatusDtoValidator
    : AbstractValidator<UpdateOrderStatusDto>
{
    public UpdateOrderStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();
    }
}