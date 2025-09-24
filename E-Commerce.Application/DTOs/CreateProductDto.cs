using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^P\d{2,}$", ErrorMessage = "Product code must be in format P01, P02, etc.")]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity must be 0 or greater")]
        public int MinimumQuantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100")]
        public double? DiscountRate { get; set; }
    }
}