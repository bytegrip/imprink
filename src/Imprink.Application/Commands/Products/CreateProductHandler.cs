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

public class CreateProductHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var product = mapper.Map<Product>(request);

            var createdProduct = await unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            
            if (createdProduct.CategoryId.HasValue)
            {
                createdProduct.Category = (await unitOfWork.CategoryRepository.GetByIdAsync(createdProduct.CategoryId.Value, cancellationToken))!;
            }
            
            await unitOfWork.SaveAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return mapper.Map<ProductDto>(createdProduct);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}