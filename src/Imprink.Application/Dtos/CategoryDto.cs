namespace Imprink.Application.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}