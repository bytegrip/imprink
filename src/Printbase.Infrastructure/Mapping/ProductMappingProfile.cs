using AutoMapper;
using Printbase.Application.Products.Commands.CreateProduct;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Products;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductDbEntity, Product>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));
            
        CreateMap<Product, ProductDbEntity>()
            .ForMember(dest => dest.Type, opt => opt.Ignore()) // Handle in repository
            .ForMember(dest => dest.Variants, opt => opt.Ignore()); // Handle in repository
        
        // ProductVariant mapping
        CreateMap<ProductVariantDbEntity, ProductVariant>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            
        CreateMap<ProductVariant, ProductVariantDbEntity>()
            .ForMember(dest => dest.Product, opt => opt.Ignore()); // Handle in repository
        
        // ProductType mapping
        CreateMap<ProductTypeDbEntity, ProductType>()
            .ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            
        CreateMap<ProductType, ProductTypeDbEntity>()
            .ForMember(dest => dest.Group, opt => opt.Ignore()) // Handle in repository
            .ForMember(dest => dest.Products, opt => opt.Ignore()); // Handle in repository
        
        // ProductGroup mapping
        CreateMap<ProductGroupDbEntity, ProductGroup>()
            .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types));
            
        CreateMap<ProductGroup, ProductGroupDbEntity>()
            .ForMember(dest => dest.Types, opt => opt.Ignore()); // Handle in repository
            
        // Domain <-> DTO mappings
        
        // Product to DTO mapping
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Type.Group.Name))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));
            
        // ProductVariant to DTO mapping
        CreateMap<ProductVariant, ProductVariantDto>();
        
        // Command to Domain mappings
        
        // CreateProductCommand to Product mapping
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
            
        // CreateProductVariantDto to ProductVariant mapping
        CreateMap<CreateProductVariantDto, ProductVariant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
    }
}