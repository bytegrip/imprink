using MediatR;
using Printbase.Application.Products.Commands;

namespace Printbase.Application.Products.Handlers;

public class DeleteCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
        try
        {
            var exists = await unitOfWork.CategoryRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync();
                return false;
            }

            await unitOfWork.CategoryRepository.DeleteAsync(request.Id, cancellationToken);
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