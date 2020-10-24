using System.Linq;
using FluentValidation;
using Insurance.Domain.Models;

namespace Insurance.Domain.Validators
{
    public class OrderDtoValidator : AbstractValidator<CalculateOrderInsuranceRequest>
    {
        public OrderDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty()
                .Must(items =>
                {
                    return items.Count == items.Select(i => i.ProductId).Distinct().Count();
                }).WithMessage("Items must be grouped by product id");
        }
    }
}