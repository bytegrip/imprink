namespace Printbase.Domain.Entities.Products;

public class ProductType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid GroupId { get; private set; }
    
    private ProductType() { }
    
    public ProductType(Guid id, string name, Guid groupId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Type name cannot be empty", nameof(name));
            
        if (groupId == Guid.Empty)
            throw new ArgumentException("Group ID cannot be empty", nameof(groupId));
            
        Id = id;
        Name = name;
        GroupId = groupId;
        Description = description;
    }
    
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Type name cannot be empty", nameof(name));
            
        Name = name;
    }
    
    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}