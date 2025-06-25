using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class ShippingStatusMappingProfile : Profile
{
    public ShippingStatusMappingProfile()
    {
        CreateMap<ShippingStatus, ShippingStatusDto>();

        CreateMap<ShippingStatusDto, ShippingStatus>();
    }
}