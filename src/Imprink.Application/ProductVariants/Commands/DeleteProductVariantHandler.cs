using MediatR;

namespace Imprink.Application.Domains.ProductVariants;

public class DeleteProductVariantCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteProductVariantHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductVariantCommand, bool>
{
    public async Task<bool> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var exists = await unitOfWork.ProductVariantRepository.ExistsAsync(request.Id, cancellationToken);
            
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return false;
            }

            await unitOfWork.ProductVariantRepository.DeleteAsync(request.Id, cancellationToken);
            
            await unitOfWork.SaveAsync(cancellationToken);
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