using System.Runtime.Serialization;

namespace CartingService.Domain.ExceptionHandling;

public class NotFoundException : Exception
{
    private const string _notFoundMessage = "{0} with the following identifier '{1}' was not found.";

    public NotFoundException()
    {
    }

    public NotFoundException(string? entityName, string? identifier) : base(string.Format(_notFoundMessage, entityName, identifier))
    {
    }

    public NotFoundException(string? entityName, string? identifier, Exception? innerException) : base(string.Format(_notFoundMessage, entityName, identifier), innerException)
    {
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
