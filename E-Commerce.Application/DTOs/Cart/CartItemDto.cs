namespace E_Commerce.Application.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int? DiscountPercentage { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}
