using AutoMapper;
using Imprink.Application.Commands.Orders;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore()) 
            .ForMember(dest => dest.OrderStatusId, opt => opt.Ignore())
            .ForMember(dest => dest.ShippingStatusId, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ShippingStatus, opt => opt.Ignore())
            .ForMember(dest => dest.OrderAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProductVariant, opt => opt.Ignore());

        CreateMap<Order, OrderDto>();

        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ShippingStatus, opt => opt.Ignore())
            .ForMember(dest => dest.OrderAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProductVariant, opt => opt.Ignore());
    }
}