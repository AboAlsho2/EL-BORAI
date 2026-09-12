using ELBORAI.Application.DTOs.Carts;
using FluentValidation;

namespace ELBORAI.Application.Validators.Cart;

public class AddToCartDtoValidator
    : AbstractValidator<AddToCartDto>
{
    public AddToCartDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}