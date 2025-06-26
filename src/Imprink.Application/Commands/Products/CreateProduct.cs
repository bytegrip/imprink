using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Products;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsCustomizable { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
}

public class CreateProduct(
    IUnitOfWork uw, 
    IMapper mapper) 
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        return await uw.TransactAsync(async () =>
        {
            var product = mapper.Map<Product>(request);
            var createdProduct = await uw.ProductRepository.AddAsync(product, cancellationToken);

            if (createdProduct.CategoryId.HasValue)
            {
                createdProduct.Category = 
                    (await uw.CategoryRepository.GetByIdAsync(createdProduct.CategoryId.Value, cancellationToken))!;
            }

            return mapper.Map<ProductDto>(createdProduct);
        }, cancellationToken);
    }
}