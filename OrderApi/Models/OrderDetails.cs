using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models;

public class OrderDetails
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailsId { get; set; }
    public string ProductId { get; set; } = string.Empty; // = Guid.NewGuid().ToString();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int Count { get; set; }
    public string ProductName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}
