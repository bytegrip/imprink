using Imprink.Application.Products.Dtos;
using MediatR;

namespace Imprink.Application.Products.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    public bool? IsActive { get; set; }
    public bool RootCategoriesOnly { get; set; } = false;
}