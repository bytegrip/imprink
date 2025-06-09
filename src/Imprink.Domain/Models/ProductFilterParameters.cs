namespace Imprink.Domain.Models;

public class ProductFilterParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? IsCustomizable { get; set; }
    public string SortBy { get; set; } = "Name";
    public string SortDirection { get; set; } = "ASC";
}