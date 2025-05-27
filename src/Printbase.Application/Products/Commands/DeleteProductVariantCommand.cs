using MediatR;

namespace Printbase.Application.Products.Commands;

public class DeleteProductVariantCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
