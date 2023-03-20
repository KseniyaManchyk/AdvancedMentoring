using CartingService.Domain;
using FluentValidation;

namespace CartingService.BLL.Validation;

public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(cart => cart.Id).NotEmpty().WithMessage("Cart Id is required.");
    }
}
