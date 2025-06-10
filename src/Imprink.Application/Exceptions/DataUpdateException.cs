namespace Imprink.Application.Exceptions;

public class DataUpdateException : BaseApplicationException
{
    public DataUpdateException(string message) : base(message) { }
    public DataUpdateException(string message, Exception innerException) : base(message, innerException) { }
}