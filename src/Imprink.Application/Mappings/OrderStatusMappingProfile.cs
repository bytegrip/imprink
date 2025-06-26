using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class OrderStatusMappingProfile : Profile
{
    public OrderStatusMappingProfile()
    {
        CreateMap<OrderStatus, OrderStatusDto>();

        CreateMap<OrderStatusDto, OrderStatus>();
    }
}