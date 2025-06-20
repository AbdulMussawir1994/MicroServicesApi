using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Dtos
{
    public class CreateOrderDetailsDto
    {
        [Required]
        public string ProductId { get; set; } = string.Empty; // = Guid.NewGuid().ToString();

        [Required]
        public int Count { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
