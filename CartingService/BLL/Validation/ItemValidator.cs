using CartingService.Domain;
using FluentValidation;

namespace CartingService.BLL.Validation
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(item => item.Id).NotEmpty().WithMessage("Item Id is required.");
            RuleFor(item => item.Name).NotEmpty().WithMessage("Item Name is required.");
            RuleFor(item => item.Price).NotEmpty().WithMessage("Item Price is required.");
        }
    }
}
