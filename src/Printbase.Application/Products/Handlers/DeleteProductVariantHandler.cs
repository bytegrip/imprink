using MediatR;
using Printbase.Application.Products.Commands;

namespace Printbase.Application.Products.Handlers;

public class DeleteProductVariantHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductVariantCommand, bool>
{
    public async Task<bool> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
        try
        {
            var exists = await unitOfWork.ProductVariantRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync();
                return false;
            }

            await unitOfWork.ProductVariantRepository.DeleteAsync(request.Id, cancellationToken);
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