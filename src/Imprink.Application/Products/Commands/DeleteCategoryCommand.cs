using MediatR;

namespace Imprink.Application.Products.Commands;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}