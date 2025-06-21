using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models;

public class OrderDetails
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailsId { get; set; }

    [Required, MaxLength(36)]
    public string ProductId { get; set; } = string.Empty; // = Guid.NewGuid().ToString();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Stock { get; set; }
    public string ProductName { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public string UserId { get; set; } = string.Empty;
    [Display(Name = "Consumer Name")]
    public string Consumer { get; set; } = string.Empty;
    [Display(Name = "Order Status")]
    public string Status { get; set; } = string.Empty;

}
