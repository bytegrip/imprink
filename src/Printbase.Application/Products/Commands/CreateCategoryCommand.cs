using MediatR;
using Printbase.Application.Products.Dtos;

namespace Printbase.Application.Products.Commands;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? ParentCategoryId { get; set; }
}