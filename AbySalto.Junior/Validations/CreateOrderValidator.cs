using AbySalto.Junior.DTO;
using FluentValidation;

namespace AbySalto.Junior.Validations;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.BuyerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DeliveryAddress).NotEmpty().MaximumLength(250);
        RuleFor(x => x.ContactNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Remark).MaximumLength(500);
        RuleFor(x => x.Items).NotEmpty().WithMessage("Order must contain at least one item.");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemId).GreaterThan(0);
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}
