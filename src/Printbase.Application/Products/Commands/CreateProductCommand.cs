using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsCustomizable { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
}