using Printbase.Application.ProductType;

namespace Printbase.Application.ProductGroup;

public class ProductGroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ProductTypeDto> Types { get; set; } = [];
}

public class ProductGroupCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class ProductGroupUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}