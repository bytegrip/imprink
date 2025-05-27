using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Queries;

public class GetProductVariantsQuery : IRequest<IEnumerable<ProductVariantDto>>
{
    public Guid? ProductId { get; set; }
    public bool? IsActive { get; set; }
    public bool InStockOnly { get; set; } = false;
}