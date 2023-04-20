using System.Runtime.Serialization;

namespace CartingService.Domain.ExceptionHandling;

public class MessageQueueConectionException : Exception
{
    private const string _connectionMessage = "Application could not connect to message queue with the name '{0}'";

    public MessageQueueConectionException()
    {
    }

    public MessageQueueConectionException(string? queueName) : base(string.Format(_connectionMessage, queueName))
    {
    }

    public MessageQueueConectionException(string? queueName, Exception? innerException) : base(string.Format(_connectionMessage, queueName), innerException)
    {
    }

    protected MessageQueueConectionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
