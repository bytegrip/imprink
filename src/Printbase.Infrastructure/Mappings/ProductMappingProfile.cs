using AutoMapper;
using Printbase.Application.Products.Commands.CreateProduct;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Products;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Product DbEntity -> Domain Entity 
        CreateMap<ProductDbEntity, Product>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Variants,
                opt => opt.MapFrom(src => src.Variants));

        // ProductGroup DbEntity -> Domain Entity
        CreateMap<ProductGroupDbEntity, ProductGroup>()
            .ForMember(dest => dest.Types,
                opt => opt.MapFrom(src => src.Types));

        // ProductType DbEntity -> Domain Entity
        CreateMap<ProductTypeDbEntity, ProductType>()
            .ForMember(dest => dest.Group,
                opt => opt.MapFrom(src => src.Group))
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Products));

        // ProductVariant DbEntity -> Domain Entity
        CreateMap<ProductVariantDbEntity, ProductVariant>()
            .ForMember(dest => dest.Product,
                opt => opt.MapFrom(src => src.Product));

        // Product Domain Entity -> DbEntity
        CreateMap<Product, ProductDbEntity>()
            .ForMember(dest => dest.Type, 
                opt => opt.Ignore()) // in repo
            .ForMember(dest => dest.Variants, 
                opt => opt.Ignore()); // in repo

        // ProductVariant Domain Entity -> DbEntity
        CreateMap<ProductVariant, ProductVariantDbEntity>()
            .ForMember(dest => dest.Product, 
                opt => opt.Ignore()); // in repo

        // ProductType Domain Entity -> DbEntity
        CreateMap<ProductType, ProductTypeDbEntity>()
            .ForMember(dest => dest.Group, 
                opt => opt.Ignore()) // in repo
            .ForMember(dest => dest.Products, 
                opt => opt.Ignore()); // in repo

        // ProductGroup Domain Entity -> DbEntity
        CreateMap<ProductGroup, ProductGroupDbEntity>()
            .ForMember(dest => dest.Types, 
                opt => opt.Ignore()); // in repo
    }
}