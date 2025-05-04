namespace Printbase.Application.Products.Commands.CreateProduct;

public class CreateProductVariantDto
{
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Stock { get; set; }
    public string? SKU { get; set; }
}