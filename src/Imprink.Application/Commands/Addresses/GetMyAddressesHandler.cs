using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Services;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Addresses;

public class GetMyAddressesQuery : IRequest<IEnumerable<AddressDto?>>;

public class GetMyAddressesHandler(
    IUnitOfWork uw, 
    IMapper mapper, 
    ICurrentUserService userService) 
    : IRequestHandler<GetMyAddressesQuery, IEnumerable<AddressDto?>>
{
    public async Task<IEnumerable<AddressDto?>> Handle(
        GetMyAddressesQuery request, 
        CancellationToken cancellationToken)
    {
        IEnumerable<Address?> addresses = await uw.AddressRepository
            .GetByUserIdAsync(userService.GetCurrentUserId(), cancellationToken);
        
        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }
}