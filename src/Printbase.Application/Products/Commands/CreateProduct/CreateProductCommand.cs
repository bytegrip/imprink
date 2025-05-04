using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid TypeId { get; set; }
    public List<CreateProductVariantDto>? Variants { get; set; }
}