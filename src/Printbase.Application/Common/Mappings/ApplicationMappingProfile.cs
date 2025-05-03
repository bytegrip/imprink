using AutoMapper;
using Printbase.Application.ProductGroup;
using Printbase.Application.Products;
using Printbase.Application.ProductType;
using Printbase.Domain.Entities.Products;

namespace Printbase.Application.Common.Mappings;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.TypeName, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));
        
        CreateMap<ProductCreateDto, Product>()
            .ConstructUsing((src, ctx) => new Product(
                Guid.NewGuid(),
                src.Name,
                src.TypeId,
                src.Description,
                src.Discount))
            .ForMember(dest => dest.Variants, opt => opt.Ignore());
        
        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore());
        
        CreateMap<ProductVariant, ProductVariantDto>()
            .ForMember(dest => dest.EffectivePrice, opt => opt.MapFrom(src => src.GetEffectivePrice(src.Discount)));
        
        CreateMap<ProductVariantCreateDto, ProductVariant>()
            .ConstructUsing((src, ctx) => new ProductVariant(
                Guid.NewGuid(),
                Guid.Empty,
                src.Price,
                src.Color,
                src.Size,
                src.Discount,
                src.Stock));
        
        CreateMap<ProductVariantUpdateDto, ProductVariant>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        
        CreateMap<Domain.Entities.Products.ProductType, ProductTypeDto>()
            .ForMember(dest => dest.GroupName, opt => opt.Ignore());
        
        CreateMap<ProductTypeCreateDto, Domain.Entities.Products.ProductType>()
            .ConstructUsing((src, ctx) => new Domain.Entities.Products.ProductType(
                Guid.NewGuid(),
                src.Name,
                src.GroupId,
                src.Description));
        
        CreateMap<ProductTypeUpdateDto, Domain.Entities.Products.ProductType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GroupId, opt => opt.Ignore());
        
        CreateMap<Domain.Entities.Products.ProductGroup, ProductGroupDto>();
        
        CreateMap<ProductGroupCreateDto, Domain.Entities.Products.ProductGroup>()
            .ConstructUsing((src, ctx) => new Domain.Entities.Products.ProductGroup(
                Guid.NewGuid(),
                src.Name,
                src.Description));
        
        CreateMap<ProductGroupUpdateDto, Domain.Entities.Products.ProductGroup>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Types, opt => opt.Ignore());
    }
}