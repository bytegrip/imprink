namespace Printbase.Application.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Discount { get; set; }
    public Guid TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public List<ProductVariantDto> Variants { get; set; } = new();
}

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Stock { get; set; }
    public decimal EffectivePrice { get; set; }
}

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Discount { get; set; }
    public Guid TypeId { get; set; }
    public List<ProductVariantCreateDto> Variants { get; set; } = new();
}

public class ProductVariantCreateDto
{
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Stock { get; set; }
}

public class ProductUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Discount { get; set; }
    public Guid TypeId { get; set; }
}

public class ProductVariantUpdateDto
{
    public Guid Id { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Stock { get; set; }
}