using Imprink.Application.Products.Dtos;
using MediatR;

namespace Imprink.Application.Products.Commands;

public class CreateProductVariantCommand : IRequest<ProductVariantDto>
{
    public Guid ProductId { get; set; }
    public string Size { get; set; } = null!;
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Sku { get; set; } = null!;
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
}