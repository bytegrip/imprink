using AutoMapper;
using Printbase.Domain.Entities.Products;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDbEntity>()
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            .ForMember(dest => dest.Type, opt => opt.Ignore());

        CreateMap<ProductDbEntity, Product>()
            .ConstructUsing((src, _) => new Product(
                src.Id,
                src.Name,
                src.TypeId,
                src.Description,
                src.Discount))
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                foreach (var dbVariant in src.Variants)
                {
                    var variant = ctx.Mapper.Map<ProductVariant>(dbVariant);
                    dest.AddVariant(variant);
                }
            });

        CreateMap<ProductVariant, ProductVariantDbEntity>()
            .ForMember(dest => dest.Product, opt => opt.Ignore());

        CreateMap<ProductVariantDbEntity, ProductVariant>()
            .ConstructUsing((src, _) => new ProductVariant(
                src.Id,
                src.ProductId,
                src.Price,
                src.Color,
                src.Size,
                src.Discount,
                src.Stock));

        CreateMap<ProductType, ProductTypeDbEntity>()
            .ForMember(dest => dest.Group, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        CreateMap<ProductTypeDbEntity, ProductType>()
            .ConstructUsing((src, _) => new ProductType(
                src.Id,
                src.Name,
                src.GroupId,
                src.Description));

        CreateMap<ProductGroup, ProductGroupDbEntity>()
            .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types));

        CreateMap<ProductGroupDbEntity, ProductGroup>()
            .ConstructUsing((src, _) => new ProductGroup(
                src.Id,
                src.Name,
                src.Description))
            .ForMember(dest => dest.Types, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                foreach (var dbType in src.Types)
                {
                    var type = ctx.Mapper.Map<ProductType>(dbType);
                    dest.AddType(type);
                }
            });
    }
}