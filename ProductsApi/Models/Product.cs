using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsApi.Models;

[Table("Product")]
public class Product
{
    [Key]
    [MaxLength(36)]
    public string ProductId { get; set; } = Guid.NewGuid().ToString();

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

    public string? ImageUrl { get; set; } // Base64, no MaxLength since it's large

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    [MaxLength(50)]
    public string? CreatedBy { get; set; } = string.Empty;
}
