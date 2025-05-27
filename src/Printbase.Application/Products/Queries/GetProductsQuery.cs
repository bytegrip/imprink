using MediatR;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Common.Models;

namespace Printbase.Application.Products.Queries;

public class GetProductsQuery : IRequest<PagedResultDto<ProductDto>>
{
    public ProductFilterParameters FilterParameters { get; set; } = new();
}