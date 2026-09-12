using ELBORAI.Application.DTOs.Cart;
using FluentValidation;

namespace ELBORAI.Application.Validators.Cart;

public class UpdateCartItemDtoValidator
    : AbstractValidator<UpdateCartItemDto>
{
    public UpdateCartItemDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}