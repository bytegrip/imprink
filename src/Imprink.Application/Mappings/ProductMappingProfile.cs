using AutoMapper;
using Imprink.Application.Commands.Products;
using Imprink.Application.Commands.ProductVariants;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class ProductMappingProfile: Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductVariantCommand, ProductVariant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        
        CreateMap<ProductVariant, ProductVariantDto>();
        CreateMap<ProductVariantDto, ProductVariant>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        
        CreateMap<Product, ProductDto>();
        
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductVariants, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        
        CreateMap<Category, CategoryDto>();
    }
}