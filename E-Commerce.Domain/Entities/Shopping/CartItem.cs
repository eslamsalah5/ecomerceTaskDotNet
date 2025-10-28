using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPriceAtAdd { get; set; }

        public int? DiscountPercentageAtAdd { get; set; }

        // Navigation properties
        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
