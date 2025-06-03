using MediatR;

namespace Imprink.Application.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}