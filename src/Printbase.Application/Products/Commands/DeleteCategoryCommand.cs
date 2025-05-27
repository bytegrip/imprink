using MediatR;

namespace Printbase.Application.Products.Commands;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}