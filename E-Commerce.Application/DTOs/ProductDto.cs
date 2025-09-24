namespace E_Commerce.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int MinimumQuantity { get; set; }
        public double? DiscountRate { get; set; }
    }
}