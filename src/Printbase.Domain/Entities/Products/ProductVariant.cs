namespace Printbase.Domain.Entities.Products;

public class ProductVariant
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string? Color { get; private set; }
    public string? Size { get; private set; }
    public decimal Price { get; private set; }
    public decimal? Discount { get; private set; }
    public int Stock { get; private set; }
    
    private ProductVariant() { }
    
    public ProductVariant(Guid id, Guid productId, decimal price, string? color = null, string? size = null, 
        decimal? discount = null, int stock = 0)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));
            
        Id = id;
        ProductId = productId;
        Color = color;
        Size = size;
        Price = price;
        Discount = discount;
        Stock = stock;
    }
    
    public void UpdateColor(string? color)
    {
        Color = color;
    }
    
    public void UpdateSize(string? size)
    {
        Size = size;
    }
    
    public void UpdatePrice(decimal price)
    {
        if (price < 0) throw new ArgumentException("Price cannot be negative", nameof(price));
            
        Price = price;
    }
    
    public void UpdateDiscount(decimal? discount)
    {
        if (discount is < 0 or > 100)
            throw new ArgumentException("Discount must be between 0 and 100", nameof(discount));
            
        Discount = discount;
    }
    
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative", nameof(quantity));
            
        Stock = quantity;
    }
    
    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to add must be positive", nameof(quantity));
            
        Stock += quantity;
    }
    
    public bool RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to remove must be positive", nameof(quantity));
            
        if (Stock < quantity) return false;
            
        Stock -= quantity;
        return true;
    }
    
    public decimal GetEffectivePrice(decimal? productDiscount = null)
    {
        var effectivePrice = Price;
        
        var discountToApply = Discount ?? productDiscount;
        
        if (discountToApply is > 0) effectivePrice -= (effectivePrice * discountToApply.Value / 100);
        
        return Math.Round(effectivePrice, 2);
    }
    
    public decimal? GetEffectiveDiscount(decimal? productDiscount = null)
    {
        return Discount ?? productDiscount;
    }
}