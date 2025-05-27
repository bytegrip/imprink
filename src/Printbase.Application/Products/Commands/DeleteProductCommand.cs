using MediatR;

namespace Printbase.Application.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}