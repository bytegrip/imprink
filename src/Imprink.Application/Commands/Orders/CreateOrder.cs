using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using Imprink.Application.Services;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Orders;

public class CreateOrderCommand : IRequest<OrderDto>
{
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
    public string[]? OriginalImageUrls { get; set; } = [];
    public string? CustomizationImageUrl { get; set; } = null!;
    public string? CustomizationDescription { get; set; } = null!;
    public Guid AddressId { get; set; }
}

public class CreateOrder(
    IUnitOfWork uw, 
    IMapper mapper, 
    ICurrentUserService userService) 
    : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(
        CreateOrderCommand request, 
        CancellationToken cancellationToken)
    {
        return await uw.TransactAsync(async () =>
        {
            var userId = userService.GetCurrentUserId()!;
            
            var sourceAddress = await uw.AddressRepository
                .GetByIdAndUserIdAsync(request.AddressId, userId, cancellationToken);
            
            if (sourceAddress == null)
                throw new NotFoundException($"Address {request.AddressId} not found for {userId}");

            var order = mapper.Map<Order>(request);
            order.UserId = userService.GetCurrentUserId();
            order.OrderDate = DateTime.UtcNow;
            order.OrderStatusId = 0; 
            order.ShippingStatusId = 0; 
            
            var variant = uw.ProductVariantRepository
                .GetByIdAsync(request.ProductVariantId, cancellationToken).Result;
            
            if (variant == null)
                throw new NotFoundException("Product variant not found");
            
            order.Amount = variant.Price * request.Quantity;


            var createdOrder = await uw.OrderRepository.AddAsync(order, cancellationToken);
            
            var orderAddress = new OrderAddress
            {
                OrderId = createdOrder.Id,
                AddressType = sourceAddress.AddressType,
                FirstName = sourceAddress.FirstName,
                LastName = sourceAddress.LastName,
                Company = sourceAddress.Company,
                AddressLine1 = sourceAddress.AddressLine1,
                AddressLine2 = sourceAddress.AddressLine2,
                ApartmentNumber = sourceAddress.ApartmentNumber,
                BuildingNumber = sourceAddress.BuildingNumber,
                Floor = sourceAddress.Floor,
                City = sourceAddress.City,
                State = sourceAddress.State,
                PostalCode = sourceAddress.PostalCode,
                Country = sourceAddress.Country,
                PhoneNumber = sourceAddress.PhoneNumber,
                Instructions = sourceAddress.Instructions,
                Order = createdOrder
            };
            
            await uw.OrderAddressRepository.AddAsync(orderAddress, cancellationToken);
            
            createdOrder.Product = (await uw.ProductRepository
                .GetByIdAsync(createdOrder.ProductId, cancellationToken))!;
            
            if (!createdOrder.ProductVariantId.HasValue) 
                throw new NotFoundException("Product variant not found");
            
            createdOrder.ProductVariant = await uw.ProductVariantRepository
                .GetByIdAsync(createdOrder.ProductVariantId.Value, cancellationToken);

            await uw.SaveAsync(cancellationToken);
            
            return mapper.Map<OrderDto>(createdOrder);
        }, cancellationToken);
    }
}
