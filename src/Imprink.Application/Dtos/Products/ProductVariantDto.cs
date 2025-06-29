namespace Imprink.Application.Dtos;

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Size { get; set; } = null!;
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Sku { get; set; } = null!;
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public ProductDto? Product { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}