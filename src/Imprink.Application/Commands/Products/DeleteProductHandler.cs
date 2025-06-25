using Imprink.Application.Exceptions;
using MediatR;

namespace Imprink.Application.Commands.Products;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProductHandler(
    IUnitOfWork uw) 
    : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(
        DeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        await uw.TransactAsync(async () =>
        {
            var exists = await uw.ProductRepository
                .ExistsAsync(request.Id, cancellationToken);

            if (!exists)
            {
                throw new NotFoundException($"Product with id {request.Id} not found");
            }
        
            await uw.ProductRepository.DeleteAsync(request.Id, cancellationToken);
        }, cancellationToken);
    }
}