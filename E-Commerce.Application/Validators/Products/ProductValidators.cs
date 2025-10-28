using FluentValidation;
using E_Commerce.Application.DTOs;

namespace E_Commerce.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product code is required")
                .Matches(@"^P\d{2,}$").WithMessage("Product code must be in format P01, P02, P123, etc.")
                .MaximumLength(10).WithMessage("Product code must not exceed 10 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100).When(x => x.DiscountPercentage.HasValue)
                .WithMessage("Discount percentage must be between 0 and 100");
        }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product code is required")
                .Matches(@"^P\d{2,}$").WithMessage("Product code must be in format P01, P02, P123, etc.")
                .MaximumLength(10).WithMessage("Product code must not exceed 10 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100).When(x => x.DiscountPercentage.HasValue)
                .WithMessage("Discount percentage must be between 0 and 100");
        }
    }
}
