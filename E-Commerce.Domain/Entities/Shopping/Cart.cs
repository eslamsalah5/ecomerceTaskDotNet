using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class Cart : BaseAuditableEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        public decimal Total { get; set; }

        // Navigation property
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        // Concurrency control
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
