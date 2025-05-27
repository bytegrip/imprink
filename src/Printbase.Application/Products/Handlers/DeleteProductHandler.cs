using MediatR;
using Printbase.Application.Products.Commands;

namespace Printbase.Application.Products.Handlers;

public class DeleteProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
        try
        {
            var exists = await unitOfWork.ProductRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync();
                return false;
            }

            await unitOfWork.ProductRepository.DeleteAsync(request.Id, cancellationToken);
            await unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}