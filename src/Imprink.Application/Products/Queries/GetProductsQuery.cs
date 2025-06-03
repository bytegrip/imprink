using Imprink.Application.Products.Dtos;
using Imprink.Domain.Common.Models;
using MediatR;

namespace Imprink.Application.Products.Queries;

public class GetProductsQuery : IRequest<PagedResultDto<ProductDto>>
{
    public ProductFilterParameters FilterParameters { get; set; } = new();
}