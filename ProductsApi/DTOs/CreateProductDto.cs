using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsApi.DTOs
{
    public class CreateProductDto
    {
        [Required, MaxLength(30)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ProductDescription { get; set; }

        [MaxLength(30)]
        public string? ProductCategory { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }

    public class ProductMessageDto
    {
        public string User { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Quantity { get; set; }
        public string Consumer { get; set; } = string.Empty;
        public IFormFile? ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
