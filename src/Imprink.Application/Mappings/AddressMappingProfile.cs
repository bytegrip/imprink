using AutoMapper;
using Imprink.Application.Commands.Addresses;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;

namespace Imprink.Application.Mappings;

public class AddressMappingProfile : Profile
{
    public AddressMappingProfile()
    {
        CreateMap<CreateAddressCommand, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Address, AddressDto>();

        CreateMap<AddressDto, Address>()
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}