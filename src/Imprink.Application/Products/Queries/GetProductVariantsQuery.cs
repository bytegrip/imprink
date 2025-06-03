using Imprink.Application.Products.Dtos;
using MediatR;

namespace Imprink.Application.Products.Queries;

public class GetProductVariantsQuery : IRequest<IEnumerable<ProductVariantDto>>
{
    public Guid? ProductId { get; set; }
    public bool? IsActive { get; set; }
    public bool InStockOnly { get; set; } = false;
}