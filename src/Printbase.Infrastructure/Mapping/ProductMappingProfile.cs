using AutoMapper;
using Printbase.Domain.Entities.Products;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductDbEntity, Product>()
            .ForMember(dest => dest.Type, 
                opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Variants, 
                opt => opt.MapFrom(src => src.Variants));
            
        CreateMap<Product, ProductDbEntity>()
            .ForMember(dest => dest.Type, 
                opt => opt.Ignore()) // in repo
            .ForMember(dest => dest.Variants, 
                opt => opt.Ignore()); // in repo
        
        CreateMap<ProductVariantDbEntity, ProductVariant>()
            .ForMember(dest => dest.Product, 
                opt => opt.MapFrom(src => src.Product));
            
        CreateMap<ProductVariant, ProductVariantDbEntity>()
            .ForMember(dest => dest.Product, 
                opt => opt.Ignore()); // in repo
        
        CreateMap<ProductTypeDbEntity, ProductType>()
            .ForMember(dest => dest.Group, 
                opt => opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Products, 
                opt => opt.MapFrom(src => src.Products));
            
        CreateMap<ProductType, ProductTypeDbEntity>()
            .ForMember(dest => dest.Group, 
                opt => opt.Ignore()) // in repo
            .ForMember(dest => dest.Products, 
                opt => opt.Ignore()); // in repo
        
        CreateMap<ProductGroupDbEntity, ProductGroup>()
            .ForMember(dest => dest.Types, 
                opt => opt.MapFrom(src => src.Types));
            
        CreateMap<ProductGroup, ProductGroupDbEntity>()
            .ForMember(dest => dest.Types, 
                opt => opt.Ignore()); // in repo
    }
}