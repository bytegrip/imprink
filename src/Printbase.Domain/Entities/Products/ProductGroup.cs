namespace Printbase.Domain.Entities.Products;

public class ProductGroup
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<ProductType> Types => _types.AsReadOnly();
    
    private readonly List<ProductType> _types = new();
    
    private ProductGroup() { }
    
    public ProductGroup(Guid id, string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name cannot be empty", nameof(name));
            
        Id = id;
        Name = name;
        Description = description;
    }
    
    public void AddType(ProductType type)
    {
        ArgumentNullException.ThrowIfNull(type);

        _types.Add(type);
    }
    
    public void RemoveType(Guid typeId)
    {
        var type = _types.Find(t => t.Id == typeId);
        if (type != null) _types.Remove(type);
    }
    
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name cannot be empty", nameof(name));
            
        Name = name;
    }
    
    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}