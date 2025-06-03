using Imprink.Application.Products.Commands;
using MediatR;

namespace Imprink.Application.Products.Handlers;

public class DeleteProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var exists = await unitOfWork.ProductRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return false;
            }

            await unitOfWork.ProductRepository.DeleteAsync(request.Id, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}