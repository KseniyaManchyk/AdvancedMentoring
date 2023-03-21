using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(item => item.Id).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(item => item.Name).NotEmpty().WithMessage("Product Name is required.");
        RuleFor(item => item.Price).NotEmpty().WithMessage("Product Price is required.");
        RuleFor(item => item.Amount).NotEmpty().WithMessage("Product Amount is required.");
        RuleFor(item => item.Amount)
            .NotEmpty().WithMessage("Product Amount is required.")
            .GreaterThan(0).WithMessage("Product Amount should be positive number.");
    }
}
