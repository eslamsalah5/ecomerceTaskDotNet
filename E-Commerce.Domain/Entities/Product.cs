using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int MinimumQuantity { get; set; }

        public double? DiscountRate { get; set; }
    }
}