using Imprink.Application.Products.Dtos;
using Imprink.Domain.Common.Models;
using MediatR;

namespace Imprink.Application.Products;

public class GetProductsQuery : IRequest<PagedResultDto<ProductDto>>
{
    public ProductFilterParameters FilterParameters { get; set; } = new();
}

public class GetProductsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductsQuery, PagedResultDto<ProductDto>>
{
    public async Task<PagedResultDto<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await unitOfWork.ProductRepository.GetPagedAsync(request.FilterParameters, cancellationToken);
        
        var productDtos = pagedResult.Items.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            BasePrice = p.BasePrice,
            IsCustomizable = p.IsCustomizable,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            CategoryId = p.CategoryId,
            Category = new CategoryDto
            {
                Id = p.Category.Id,
                Name = p.Category.Name,
                Description = p.Category.Description,
                ImageUrl = p.Category.ImageUrl,
                SortOrder = p.Category.SortOrder,
                IsActive = p.Category.IsActive,
                ParentCategoryId = p.Category.ParentCategoryId,
                CreatedAt = p.Category.CreatedAt,
                ModifiedAt = p.Category.ModifiedAt
            },
            CreatedAt = p.CreatedAt,
            ModifiedAt = p.ModifiedAt
        });

        return new PagedResultDto<ProductDto>
        {
            Items = productDtos,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
    }
}