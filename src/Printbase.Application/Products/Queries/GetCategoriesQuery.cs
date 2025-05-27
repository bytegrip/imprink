using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    public bool? IsActive { get; set; }
    public bool RootCategoriesOnly { get; set; } = false;
}