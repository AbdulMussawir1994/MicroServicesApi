using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Dtos
{
    public class CreateOrderDetailsDto
    {
        [Required, MaxLength(36)]
        public string ProductId { get; set; } = string.Empty; // = Guid.NewGuid().ToString();

        [Required]
        public decimal Stock { get; set; }
        [Required]
        public string ProductName { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Consumer Name")]
        public string Consumer { get; set; } = string.Empty;
        [Display(Name = "Order Status")]
        public string Status { get; set; } = string.Empty;
    }
}
