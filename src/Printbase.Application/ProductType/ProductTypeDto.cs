namespace Printbase.Application.ProductType;

public class ProductTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
}

public class ProductTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid GroupId { get; set; }
}

public class ProductTypeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}