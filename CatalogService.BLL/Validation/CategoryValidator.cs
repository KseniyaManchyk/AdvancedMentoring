using CatalogService.Domain.Models;
using FluentValidation;

namespace CatalogService.BLL.Validation;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(item => item.Id).NotEmpty().WithMessage("Category Id is required.");
        RuleFor(item => item.Name).NotEmpty().WithMessage("Category Name is required.");
    }
}
