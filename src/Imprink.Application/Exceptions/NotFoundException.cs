namespace Imprink.Application.Exceptions;

public class NotFoundException : BaseApplicationException
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}