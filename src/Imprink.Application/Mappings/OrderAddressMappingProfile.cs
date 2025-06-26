using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class OrderAddressMappingProfile : Profile
{
    public OrderAddressMappingProfile()
    {
        CreateMap<OrderAddress, OrderAddressDto>();

        CreateMap<OrderAddressDto, OrderAddress>()
            .ForMember(dest => dest.Order, opt => opt.Ignore());

        CreateMap<Address, OrderAddress>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
            .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
            .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.ApartmentNumber))
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
            .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions));
    }
}