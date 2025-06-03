using MediatR;

namespace Imprink.Application.Products.Commands;

public class DeleteProductVariantCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
