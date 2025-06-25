using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Services;
using Imprink.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Imprink.Application.Commands.Addresses;

public class CreateAddressCommand : IRequest<AddressDto>
{
    public string AddressType { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Instructions { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateAddressHandler(
    IUnitOfWork uw, 
    IMapper mapper, 
    ICurrentUserService userService, 
    ILogger<CreateAddressHandler> logger) 
    : IRequestHandler<CreateAddressCommand, AddressDto>
{
    public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        return await uw.TransactAsync(async () =>
        {
            var address = mapper.Map<Address>(request);
            
            address.UserId = userService.GetCurrentUserId()!;
            
            if (address.IsDefault)
            {
                var currentDefault = await uw.AddressRepository.GetDefaultByUserIdAsync(address.UserId, cancellationToken);
                if (currentDefault != null)
                {
                    currentDefault.IsDefault = false;
                    await uw.AddressRepository.UpdateAsync(currentDefault, cancellationToken);
                }
            }
            
            var createdAddress = await uw.AddressRepository.AddAsync(address, cancellationToken);
            
            await uw.SaveAsync(cancellationToken);
            return mapper.Map<AddressDto>(createdAddress);
        }, cancellationToken);
    }
}