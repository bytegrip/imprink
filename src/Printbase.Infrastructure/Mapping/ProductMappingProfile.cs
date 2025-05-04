using AutoMapper;
using Printbase.Domain.Entities.Products;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductDbEntity, Product>();
        CreateMap<ProductGroupDbEntity, ProductGroup>();
        CreateMap<ProductTypeDbEntity, ProductType>();
        CreateMap<ProductVariantDbEntity, ProductVariant>();
        
        CreateMap<Product, ProductDbEntity>();
        CreateMap<ProductGroup, ProductGroupDbEntity>();
        CreateMap<ProductType, ProductTypeDbEntity>();
        CreateMap<ProductVariant, ProductVariantDbEntity>();
    }
}