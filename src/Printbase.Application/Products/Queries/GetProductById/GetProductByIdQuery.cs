using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Queries.GetProductById;

public class GetProductByIdQuery(Guid id, bool includeVariants = true) : IRequest<ProductDto?>
{
    public Guid Id { get; } = id;
    public bool IncludeVariants { get; } = includeVariants;
}