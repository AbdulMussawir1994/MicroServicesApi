namespace KmacHelper.ConsumerModel;

public class ProductDto
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductCategory { get; set; }
    public decimal ProductPrice { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; } = string.Empty; // UserId
    public string? UpdatedBy { get; set; } = string.Empty;
}
