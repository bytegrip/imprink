using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Addresses;

public class GetAddressByIdQuery : IRequest<AddressDto?>
{
    public Guid Id { get; set; }
    public string? UserId { get; set; } 
}

public class GetAddressByIdHandler(IUnitOfWork uw, IMapper mapper) : IRequestHandler<GetAddressByIdQuery, AddressDto?>
{
    public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        Address? address;
        
        if (!string.IsNullOrEmpty(request.UserId))
        {
            address = await uw.AddressRepository.GetByIdAndUserIdAsync(request.Id, request.UserId, cancellationToken);
        }
        else
        {
            address = await uw.AddressRepository.GetByIdAsync(request.Id, cancellationToken);
        }
        
        return address != null ? mapper.Map<AddressDto>(address) : null;
    }
}