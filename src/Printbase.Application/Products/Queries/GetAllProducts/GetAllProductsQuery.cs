using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Queries.GetAllProducts;

public class GetAllProductsQuery(bool includeVariants = true) : IRequest<AllProductsDto?>
{
    public bool IncludeVariants { get; } = includeVariants;
}