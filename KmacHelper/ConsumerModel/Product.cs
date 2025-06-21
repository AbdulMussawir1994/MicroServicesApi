namespace KmacHelper.ConsumerModel;

public class Product
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductCategory { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string User { get; set; }
    public string Consumer { get; set; }
}
