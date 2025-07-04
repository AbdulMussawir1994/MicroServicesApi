﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApi.Models;

public class OrderDetails
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
    [Required, MaxLength(50)]
    public string ProductId { get; set; } = string.Empty; // = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string ProductName { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal TotalOrders { get; set; }

    [Display(Name = "Consumer Name")]
    public string Consumer { get; set; } = string.Empty;
    [Display(Name = "Order Status")]
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

}
