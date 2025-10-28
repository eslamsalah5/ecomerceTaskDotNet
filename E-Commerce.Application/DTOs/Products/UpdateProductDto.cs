using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.DTOs
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Category is required")]
        [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product code is required")]
        [MaxLength(10, ErrorMessage = "Product code cannot exceed 10 characters")]
        [RegularExpression(@"^P\d{2,}$", ErrorMessage = "Product code must be in format P01, P02, P123, etc.")]
        public string ProductCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100")]
        public int? DiscountPercentage { get; set; }
    }
}