using FluentValidation;
using E_Commerce.Application.DTOs;

namespace E_Commerce.Application.Validators
{
    public class AddCartItemDtoValidator : AbstractValidator<AddCartItemDto>
    {
        public AddCartItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }

    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }
}
