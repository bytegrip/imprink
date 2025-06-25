using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Addresses;

public class GetAddressesByUserIdQuery : IRequest<IEnumerable<AddressDto>>
{
    public string UserId { get; set; } = null!;
    public bool ActiveOnly { get; set; } = false;
    public string? AddressType { get; set; }
}

public class GetAddressesByUserIdHandler(IUnitOfWork uw, IMapper mapper) : IRequestHandler<GetAddressesByUserIdQuery, IEnumerable<AddressDto>>
{
    public async Task<IEnumerable<AddressDto>> Handle(GetAddressesByUserIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Address> addresses;
        
        if (!string.IsNullOrEmpty(request.AddressType))
        {
            addresses = await uw.AddressRepository.GetByUserIdAndTypeAsync(request.UserId, request.AddressType, cancellationToken);
        }
        else if (request.ActiveOnly)
        {
            addresses = await uw.AddressRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);
        }
        else
        {
            addresses = await uw.AddressRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        }
        
        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }
}
