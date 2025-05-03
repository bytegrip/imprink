namespace Printbase.Domain.Entities.Products;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal? Discount { get; private set; }
    public Guid TypeId { get; private set; }
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();
    
    private readonly List<ProductVariant> _variants = [];
    
    private Product() { }
    
    public Product(Guid id, string name, Guid typeId, string? description = null, decimal? discount = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        
        if (typeId == Guid.Empty)
            throw new ArgumentException("Type ID cannot be empty", nameof(typeId));
            
        Id = id;
        Name = name;
        TypeId = typeId;
        Description = description;
        Discount = discount;
    }
    
    public void AddVariant(ProductVariant variant)
    {
        ArgumentNullException.ThrowIfNull(variant);

        _variants.Add(variant);
    }
    
    public void RemoveVariant(Guid variantId)
    {
        var variant = _variants.Find(v => v.Id == variantId);
        if (variant != null)
        {
            _variants.Remove(variant);
        }
    }
    
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
            
        Name = name;
    }
    
    public void UpdateDescription(string? description)
    {
        Description = description;
    }
    
    public void UpdateDiscount(decimal? discount)
    {
        if (discount.HasValue && (discount.Value < 0 || discount.Value > 100))
            throw new ArgumentException("Discount must be between 0 and 100", nameof(discount));
            
        Discount = discount;
    }
    
    public void UpdateType(Guid typeId)
    {
        if (typeId == Guid.Empty)
            throw new ArgumentException("Type ID cannot be empty", nameof(typeId));
            
        TypeId = typeId;
    }
    
    public decimal? GetEffectiveDiscount()
    {
        return Discount;
    }
}