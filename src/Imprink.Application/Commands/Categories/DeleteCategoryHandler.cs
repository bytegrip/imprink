using MediatR;

namespace Imprink.Application.Commands.Categories;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var exists = await unitOfWork.CategoryRepository.ExistsAsync(request.Id, cancellationToken);
            if (!exists)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return false;
            }

            await unitOfWork.CategoryRepository.DeleteAsync(request.Id, cancellationToken);
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